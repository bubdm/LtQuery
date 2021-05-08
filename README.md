# LtQuery
![Build status](https://github.com/maikami/LtQuery/actions/workflows/main.yml/badge.svg)

LtQuery is a ORM focus on Easy-to-use and Performance. 

LtQuery does not accept the input of SQL which is a string.
Instead, call giving a diverty, tiny query object.

```
var blogQuery = Lt.Query<Blog>().Where(_ => _.Id == Lt.Arg<int>());
var blog = connection.QuerySingle(blogQuery, new { Id = blogId });

var postsQuery = blogQuery.To<Post>(_ => _.BlogId);
var posts = connection.Query(postsQuery, new{ Id = blogId });
```

# Benchmark

'benchmarks/OrmPerformanceTests' result.  
May be, wrong the settings.
I don't think it's so fast.

## Measurement environment
- Windows
- SQL Server 2019 Express on local.
- LtQuery 0.1.0, Dapper 2.0.78, EFCore 5.0.5

## SelectOne from Single table
|  Method |      Mean |    Error |   StdDev |    Median |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------- |----------:|---------:|---------:|----------:|-------:|------:|------:|----------:|
| LtQuery |  97.09 μs | 1.926 μs | 5.206 μs |  94.22 μs | 0.4883 |     - |     - |   2.21 KB |
|  Dapper | 139.58 μs | 2.727 μs | 3.911 μs | 139.39 μs | 0.9766 |     - |     - |   4.37 KB |
|  EFCore | 190.78 μs | 1.087 μs | 1.017 μs | 190.50 μs | 2.6855 |     - |     - |   11.3 KB |

## SelectAll(10,000) from Single table
|  Method |      Mean |     Error |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|-------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| LtQuery |  7.050 ms | 0.0691 ms | 0.0679 ms | 515.6250 |        - |        - |   2.06 MB |
|  Dapper | 10.717 ms | 0.1404 ms | 0.1314 ms | 390.6250 | 156.2500 |  62.5000 |   2.31 MB |
|  EFCore | 11.375 ms | 0.0497 ms | 0.0415 ms | 796.8750 | 281.2500 | 171.8750 |   4.69 MB |

# Performance-aware code
In LtQuery, when the user holds the query object, 
the optimized process is executed when the second time Later.

```
// hold query object
private Query<Blog> _query;

public Blog Find(int id)
{
  if(_query == null)
    _query = Lt.Query<Blog>().Where(_ => _.Id == Lt.Arg<int>()).ToImmutable();
  return _connection.QuerySingle(_query, new{ Id = id });
}
```

# Query-Object and Method Responsibility Segregation
In LtQuery, Query-Object may differ from the general concept.  
Query-Object represents the "Region" of target data.  
That is, The count function in SQL is not included in Query-Object.

![LtQuery](https://user-images.githubusercontent.com/3863674/115135321-b4710400-a052-11eb-8781-7e7783b01163.png)

# Policy
Aiming for the best ORM for DDD which compromises that eliminates infrastructure dependence but relies on Lazy-Loading.
