var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "contacts",
    pattern: "contacts/count",
    defaults: new { controller = "Contacts", action = "ListCount" });

app.MapControllerRoute(
    name: "editcontact",
    pattern: "contacts/{id}/edit",
    defaults: new { controller = "Contacts", action = "EditContact" });

app.MapControllerRoute(
    name: "viewcontact",
    pattern: "contacts/{id}",
    defaults: new { controller = "Contacts", action = "ViewContact" });

app.MapControllerRoute(
    name: "contacts",
    pattern: "contacts",
    defaults: new { controller = "Contacts", action = "List" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
