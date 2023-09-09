# Neo4j 002 Six Degrees of Separation

This project is based on this post by Max De Marzi, [How You're Connected to Kevin Bacon](https://maxdemarzi.com/2012/01/05/how-youre-connected-to-kevin-bacon/). This project uses C# and the [Neo4j Driver (Simple)](https://github.com/neo4j/neo4j-dotnet-driver).

## Getting started on your machine

### Installing Neo4j
#### For Mac OSX (using Homebrew)
```bash
~ brew install neo4j
```

#### For Windows (using Chocolatey)
```powershell
PS C:\Users\demouser> choco install neo4j-community
```

### Starting Neo4j 
#### For Mac OSX (using Homebrew)
```bash
~ neo4j start
```

#### For Windows (using Chocolatey)
```powershell
PS C:\tools\neo4j-community\neo4j-community-5.11.0> bin\neo4j.bat start 
```

### Stopping Neo4j
#### For Mac OSX (using Homebrew)
```bash
~ neo4j stop
```

#### For Windows (using Chocolatey)
```powershell
PS C:\tools\neo4j-community\neo4j-community-5.11.0> bin\neo4j.bat stop 
```

## Run the app

#### For Mac OSX (using Homebrew)
```bash
~ cd Neo4j_002_SixDegrees
Neo4j_002_SixDegrees dotnet run
```

#### For Windows (using Chocolatey)
```powershell
PS C:\> cd Neo4j_002_SixDegrees
PS C:\Neo4j_002_SixDegrees> dotnet run
```
