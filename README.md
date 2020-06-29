# GraphQL.Maybe

[![NuGet version (GraphQL.Maybe)](https://img.shields.io/nuget/v/GraphQL.Maybe.svg?style=flat-square)](https://www.nuget.org/packages/GraphQL.Maybe/)

The `GraphQL.Maybe` type can be used to represent non-nullable graphql fields as optional fields in C#

```graphql
type User {
  id: Int!
  username: String!
  email: String
  age: Int
}
```

```C#
public class User 
{
  int Id { get; set; }
  string Username { get; set; }
  Maybe<string> Email { get; set; }
  Maybe<int> Age { get; set; }
}
```

JSON custom converters will only pass fields that are either non-optional or have a value assigned.