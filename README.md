# AutoOption
[![.NET](https://github.com/sasan-salem/AutoOption/actions/workflows/dotnet.yml/badge.svg)](https://github.com/sasan-salem/AutoOption/actions/workflows/dotnet.yml)
[![NuGet Version](https://img.shields.io/nuget/v/autooption)](https://www.nuget.org/packages/AutoOption/) 
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/sasan-salem/AutoOption/blob/master/LICENSE)<br/>
A simple library to generate options page for websites<br/><br/>
![autooption](https://github.com/sasan-salem/AutoOption/blob/preper-readme/asset/icon-small.jpg)
<br/>
- [Why](#why)
- [Demo](#demo)
- [Installation](#installation)
- [Setup](#setup)
- [Usage](#usage)

## Why
In every website, we need an options page (or setting page) where users can change some configs inside. Such as the number of rows in the lists, allowing comments on posts or not, changing themes and any other things.<br/>
To have any of those inputs in your options page, you should create a row in your table, create an input in the front-end, write the logic for that in the back-end and inserting its value in DB when every user clicks on the save button.<br/><br/>
With AutoOption you can create any input inside your options page in a couple of seconds<br/>
## Demo
![autooption](https://github.com/sasan-salem/AutoOption/blob/preper-readme/asset/demo.gif)
## Installation
Install [AutoOption](https://www.nuget.org/packages/AutoOption/) and [AutoOption.Pages](https://www.nuget.org/packages/AutoOption.Pages/) from NuGet to your project<br/>
## Setup
(If you have Database and ConnectionString, skip the step 1 and 2)
1. **Create a Database**
2. **Create a ConnectionString**<br/>
Create a ConnectionString the way you want or like my ConnectionString in appsettings.json ([like here](https://github.com/sasan-salem/AutoOption/blob/b038966cf7140761214827744681787b52ac7a96/sample/AutoOption.Demo/appsettings.json#L10))
3. **Options Class**<br/>
Create an empty class with `Options` name wherever you want
4. **Options Page**<br/>
     
     1. Right click on `Pages` folder, `Add`, `Razor Page` and choose `Razor Page - Empty`. then write `Options` for name or whatever you want
     2. In `Options.cshtml.cs` delete `OnGet()` function and add these two lines in `OptionsModel` class:

          ``` C#
          [BindProperty]
          public IFormCollection Inputs { get; set; }
          ```
     3. delete all code inside the `Options.cshtml` and add this code instead:

         ``` razor
          @page
          @addTagHelper *, AutoOption.Pages
          @model OptionsModel

          <vc:option inputs="@Model.Inputs"></vc:option>
         ```
5. **Config**<br/>
in `Startup.cs` set your `ConnectionString` to `OptionHelper.Config`
    ``` C#
    OptionHelper.Config(typeof(Options), new SqlWrapper(Configuration.GetConnectionString("DefaultConnectionString")));
    ```
## Usage
Now everything is ready. You can add any input to your options page just by writing properties in the options class.<br/>
Let's add a field of number and text type. You need to just add these two properties in your options class
``` C#
public int PageCount { get; set; }
public bool AllowComment { get; set; }
```
Now build your project and go to https://localhost:44378/Options<br/>
Done.<br/>
<br/>
If you want to have pretty titles, use `Display` attribute at top of the properties
``` C#
[Display(Name = "Page Count")]
public int PageCount { get; set; }

[Display(Name = "Allow Comment")]
public bool AllowComment { get; set; }
```
<br/>If you want to access the value of fields programmatically, write like this
```C#
int count = OptionHelper.Get<Options>().PageCount;
```
<br/>If you want to change the value of fields programmatically, write like this
```C#
OptionHelper.Set(() => OptionHelper.Get<Options>().PageCount, 10);
```

<br/>Also, you can add enum property and you will see a Select input equivalently. ([like here](https://github.com/sasan-salem/AutoOption/blob/5223c00db47b1aecd4b184cff33fe1948ab75e5a/sample/AutoOption.Demo/Options.cs#L20))
