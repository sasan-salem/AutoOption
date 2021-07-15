# AutoOption
[![.NET](https://github.com/sasan-salem/AutoOption/actions/workflows/dotnet.yml/badge.svg)](https://github.com/sasan-salem/AutoOption/actions/workflows/dotnet.yml)
[![NuGet Version](https://img.shields.io/nuget/v/autooption)](https://www.nuget.org/packages/AutoOption/) 
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/sasan-salem/AutoOption/blob/master/LICENSE)<br/>
A simple library to generate option page for websites
<br/>
- [Why](#why)
- [Installation](#installation)
- [Setup](#setup)

## Why
In every website, We need an options page (or setting page) where users can change some configs inside. Such as the number of rows in the lists, allowing comments on posts or not, changing themes and any other things.<br/>
To have any of those inputs in your options page, you should create a row in your table, create an input in the front-end, write the logic for that in the back-end and inserting its value in DB when every user clicks on the save button.<br/><br/>
With AutoOption you can create any input inside your options page in a couple of seconds
## Installation
Install [AutoOption](https://www.nuget.org/packages/AutoOption/) and [AutoOption.Pages](https://www.nuget.org/packages/AutoOption.Pages/) from NuGet to your project<br/>
## Setup
1. **Create a database**
2. **Create a ConnectionString**<br/>
Create a ConnectionString the way you want or like my ConnectionString in appsettings.json ([see the code](https://github.com/sasan-salem/AutoOption/blob/b038966cf7140761214827744681787b52ac7a96/sample/AutoOption.Demo/appsettings.json#L10))
3. **Options class**<br/>
Create an empty class with Options name wherever you want
4. **Options Page**<br/>

     - Right click on "Pages" folder, Add, Razor Page and choose Razor Page - Empty. then write "Options" for name or whatever you want
     - In Options.cshtml.cs delete OnGet() function
     - Add these two lines in OptionsModel class:
          ``` C#
          [BindProperty]
          public IFormCollection Inputs { get; set; }
          ```
     - delete all code inside the Options.cshtml and add this code instead:
         ``` razor
          @page
          @addTagHelper *, AutoOption.Pages
          @model OptionsModel

          <vc:option inputs="@Model.Inputs"></vc:option>
         ```
5. **Config**<br/>
in Startup.cs set your ConnectionString to OptionHelper.Config
    ``` C#
    OptionHelper.Config(typeof(Options), new SqlWrapper(Configuration.GetConnectionString("DefaultConnectionString")));
    ```
