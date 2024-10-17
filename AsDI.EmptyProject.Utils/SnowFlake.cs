using AsDI.Attributes;
using AsDI.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.EmptyProject.Utils
{
    [Include]
    public class SnowflakeIdGenerator
    {
        private const ulong Twepoch = 1288834974657; // 起始时间 (2010-11-04 09:42:54.657)
        private const int WorkerIdBits = 5; // 机器ID所占的位数
        private const int DatacenterIdBits = 5; // 数据中心ID所占的位数
        private const int SequenceBits = 12; // 序列所占的位数

        private const ulong MaxWorkerId = -1L ^ (-1L << WorkerIdBits); // 机器ID的最大值
        private const ulong MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits); // 数据中心ID的最大值
        private const ulong SequenceMask = -1L ^ (-1L << SequenceBits); // 序列号的掩码
        private const ulong WorkerIdShift = SequenceBits; // 机器ID左移位数
        private const ulong DatacenterIdShift = SequenceBits + WorkerIdBits; // 数据中心ID左移位数
        private const ulong TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits; // 时间戳左移位数

        private ulong _lastTimestamp = 0;
        private ulong _sequence = 0;

        private readonly object _lock = new();
        private readonly ulong _workerId; // 机器ID
        private readonly ulong _datacenterId; // 数据中心ID

        public SnowflakeIdGenerator([Value("AsDI.Snowflake.WorkId", Default = 1UL)] ulong workerId, [Value("AsDI.Snowflake.DataCenterId", Default = 1UL)] ulong datacenterId)
        {
            if (workerId > MaxWorkerId)
                throw new ArgumentException($"worker Id can't be greater than {MaxWorkerId}");
            if (datacenterId > MaxDatacenterId)
                throw new ArgumentException($"datacenter Id can't be greater than {MaxDatacenterId}");

            _workerId = workerId;
            _datacenterId = datacenterId;
        }

        public ulong NextId()
        {
            lock (_lock)
            {
                ulong timestamp = TimeGen();
                if (_lastTimestamp > timestamp)
                    throw new ApplicationException($"Clock moved backwards, refusing to generate id for {_lastTimestamp - timestamp} milliseconds");

                if (_lastTimestamp == timestamp)
                {
                    _sequence = (_sequence + 1) & SequenceMask;
                    if (_sequence == 0)
                        timestamp = TilNextMillis(_lastTimestamp);
                }
                else
                {
                    _sequence = 0;
                }

                _lastTimestamp = timestamp;

                ulong id = ((timestamp - Twepoch) << (int)TimestampLeftShift) |
                           (_datacenterId << (int)DatacenterIdShift) |
                           (_workerId << (int)WorkerIdShift) | _sequence;

                return id;
            }
        }

        public string Next()
        {
            return NextId() + "";
        }

        private ulong TilNextMillis(ulong lastTimestamp)
        {
            ulong timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
                timestamp = TimeGen();
            return timestamp;
        }

        private ulong TimeGen()
        {
            return (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
