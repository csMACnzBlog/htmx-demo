# HTMX Demo

This is an htmx demo. You can play around at https://blog.csmac.nz/htmx-demo/.

This is a demo of functionality using [htmx](https://htmx.org/) over static pages. Because it is using static pages, there are places where `GET` is used instead of `POST`, `PUT`, or `DELETE` to make the demo appear to work. Specifically, the form is completely smoke-and-mirrors where no server exists to run validations or accept the response.

Tools used:

* HTMX - https://htmx.org/
* Pico CSS - https://picocss.com/
* https://github.com/khalidabuhakmeh/Htmx.Net (for dotnet app)

There is a dotnet app in here as well with more sophisticated serverside iteraction flows.

This was built using `dotnet new mvc` in dotnet 7 and stripping out jquery, bootstrap and css, to replace it with HTMX and Pico CSS.

To dev/test use the command `dotnet watch run` from the dotnet-demo directory.
