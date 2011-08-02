# Beeline &mdash; MVC routing via attributes
Beeline provides an attribute-based approach for adding custom routes in ASP.NET MVC projects. Specifically, a URL pattern can be attached directly to an action method via an attribute instead of the global routing table.

For example:

```c#
public class WidgetController : Controller
{
    [Route("widgets/{id}")]
    public ActionResponse Show(Guid id)
    {
        // ...
    }
}
```

## Why on Earth would I want to do this?
First of all, Beeline does not change the way routing works in MVC. It only distributes routing table definitions to the action method scope instead of global scope. This has several implications, which some may find advantageous:

### Decouple URL patterns from controllers
In an ideal project, controllers are cohesive modules for tightly related logic. This can quickly break down, however, when mapping controller actions to URLs. If it turns out that two or more controllers need to share a root URL (e.g., when designing a RESTful API), the default route must be traded for a custom routing scheme.

While centralized routing works perfectly well, the routing table can become unruly as a project grows in size. Furthermore, there is always a disconnect between a URL pattern and its action method when routes are defined in `Global.asax.cs`.

This is where Beeline comes in. By moving the routing table definitions to the action method level, it triggers a subtle mindshift in controller development. We no longer try to structure our controllers or name our action methods to satisfy our desired URL schemes. Instead, we structure and name in a way that makes sense for our application logic and at the same time, we assign URL patterns in a way that makes sense for our public API.

### Code-as-documentation for URL patterns
When you generate a controller using Visual Studio, it adds comments to the controller's action methods that specify the HTTP method used and the URL pattern followed. For example:

```c#
public class AccountController : Controller
{
    // POST: /Account/LogOn/
    [HttpPost]
    public ActionResult LogOn(LogOnModel model) { /* ... */ }
}
```

This is very useful documentation from a maintenance perspective, but what happens when we change this with a custom route? We have to remember to update the documentation for every affected action method, which may or may not happen.

So instead of making this a comment, why not specify the route in this exact spot? That way when the URL changes, the documentation is forcibly updated, by nature of the fact that the route definition _is_ the documentation.

```c#
public class AccountController : Controller
{
    [HttpPost, Route("Account/LogOn")]
    public ActionResult LogOn(LogOnModel model) { /* ... */ }
}
```

This arguably clearer and helps us live DRY.

## API Documentation and Examples

See the [Wiki][wiki] for more documentation and examples.

## License

This project is licensed under the [MIT license][mit]. See LICENSE for details.

[mit]: http://www.opensource.org/licenses/mit-license.html
[wiki]: https://github.com/jgoz/beeline/wiki