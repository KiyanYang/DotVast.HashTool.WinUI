```

BenchmarkDotNet v0.13.6, Windows 11 (10.0.22621.1992/22H2/2022Update/SunValley2)
AMD Ryzen 7 6800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.100-preview.6.23330.14
  [Host]     : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
  Job-XJDCQM : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2

Runtime=.NET 7.0  Arguments=/p:SolutionName=DotVast.HashTool.Benchmark  IterationTime=512.0000 ms  

```
|              Method |  Length |       Mean | Ratio | Allocated | Alloc Ratio |
|-------------------- |-------- |-----------:|------:|----------:|------------:|
|   Native_BLAKE2b512 | 1000000 |   889.3 μs |  0.99 |     865 B |        0.52 |
| HashLib4_BLAKE2b512 | 1000000 |   899.3 μs |  1.00 |    1665 B |        1.00 |
|                     |         |            |       |           |             |
|   Native_BLAKE2s256 | 1000000 | 1,434.0 μs |  0.93 |     770 B |        0.55 |
| HashLib4_BLAKE2s256 | 1000000 | 1,537.4 μs |  1.00 |    1410 B |        1.00 |
|                     |         |            |       |           |             |
|       Native_BLAKE3 | 1000000 |   290.7 μs |  0.13 |     768 B |       0.005 |
|     HashLib4_BLAKE3 | 1000000 | 2,311.5 μs |  1.00 |  155283 B |       1.000 |
|                     |         |            |       |           |             |
|        HashLib4_MD4 | 1000000 |   822.8 μs |  1.00 |     865 B |        1.00 |
|          Native_MD4 | 1000000 |   847.3 μs |  1.03 |     721 B |        0.83 |
|                     |         |            |       |           |             |
|          Native_MD5 | 1000000 | 1,287.0 μs |  0.87 |     722 B |        1.00 |
|          DotNet_MD5 | 1000000 | 1,487.9 μs |  1.00 |     722 B |        1.00 |
|        HashLib4_MD5 | 1000000 | 2,032.9 μs |  1.37 |     866 B |        1.20 |
|                     |         |            |       |           |             |
|    Native_RIPEMD160 | 1000000 | 2,702.9 μs |  0.56 |     747 B |        0.81 |
|  HashLib4_RIPEMD160 | 1000000 | 4,786.7 μs |  1.00 |     917 B |        1.00 |
|                     |         |            |       |           |             |
|         Native_SHA1 | 1000000 |   417.9 μs |  0.35 |     744 B |        1.00 |
|         DotNet_SHA1 | 1000000 | 1,183.1 μs |  1.00 |     745 B |        1.00 |
|       HashLib4_SHA1 | 1000000 | 2,402.5 μs |  2.03 |     915 B |        1.23 |
|                     |         |            |       |           |             |
|       Native_SHA512 | 1000000 | 1,331.9 μs |  0.96 |     866 B |        1.00 |
|       DotNet_SHA512 | 1000000 | 1,384.3 μs |  1.00 |     866 B |        1.00 |
|     HashLib4_SHA512 | 1000000 | 7,254.7 μs |  5.24 |    1312 B |        1.52 |
|                     |         |            |       |           |             |
|   HashLib4_SHA3_512 | 1000000 | 3,686.9 μs |  1.00 |   22084 B |        1.00 |
|     Native_SHA3_512 | 1000000 | 3,886.4 μs |  1.05 |     868 B |        0.04 |
|                     |         |            |       |           |             |
|          Native_SM3 | 1000000 | 2,401.8 μs |  0.64 |     771 B |        1.00 |
|            Core_SM3 | 1000000 | 3,766.4 μs |  1.00 |     772 B |        1.00 |
