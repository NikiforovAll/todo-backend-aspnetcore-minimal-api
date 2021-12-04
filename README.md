# todobackend - Minimal API and .NET 6

An implementation of <https://todobackend.com> based on .NET and Minimal API approach.

<https://todobackend-aspnetcore.herokuapp.com/swagger/index.html>

## Run tests from API spec

* <https://todobackend.com/specs/index.html>
* <https://todobackend.com/specs/index.html?https://todobackend-aspnetcore.herokuapp.com/api/todos>

## Deploy to Heroku

```bash
heroku create todobackend-aspnetcore
heroku buildpacks:set jincod/dotnetcore -a todobackend-aspnetcore
# Buildpack set. Next release on todobackend-aspnetcore will use jincod/dotnetcore.
# Run git push heroku main to create a new release using this buildpack.
git remote add heroku https://git.heroku.com/todobackend-aspnetcore.git
git push heroku main
```
