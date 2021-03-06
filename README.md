## Package Installer

A Visual Studio extension that makes it easy and fast to install
Bower, npm, JSPM, TSD and NuGet packages.

[![Build status](https://ci.appveyor.com/api/projects/status/bd4o6iumw9vwf8kh?svg=true)](https://ci.appveyor.com/project/madskristensen/packageinstaller)

Download the extension at the
[VS Gallery](https://visualstudiogallery.msdn.microsoft.com/753b9720-1638-4f9a-ad8d-2c45a410fd74)
or get the
[nightly build](http://vsixgallery.com/extension/fdd64809-376e-4542-92ce-808a8df06bcc/)

See the
[changelog](CHANGELOG.md)
for changes and roadmap.

### Features

- Supports Bower, npm, JSPM, TSD (DefinatelyTyped) and NuGet
- Works for all project types including ASP.NET 5, WebForms, Website projects and more
- Intellisense for package names and versions
- Automatically creates package.json or bower.json if missing
- Automatically creates gulp-/grunt-/brocfile.js if they are being installed using npm
- Selects the last used package manager
- Customize arguments passed to each package manager

### Install a package

Simply right-click the project and select "Quick Install Package..."
to pop open the installer dialog box.

![Context menu](art/context-menu.png)

### Choose package manager

Select which package manager to use.

![auto completion](art/dialog.png)

You choice is remembered for next time you open the dialog.

### Auto completion

You get full auto completion for all package names available
in the Bower, npm, JSPM, TSD and NuGet registries.

![auto completion](art/dialog-names.png)

Also for version numbers for both Bower and npm:

![auto completion](art/dialog-versions.png)

### DefinatelyTyped package manager (TSD)
You must install TSD using npm in order for the TSD package manager
to work.

Open a console and type the following command:

```cmd
npm install tsd -g
```

Now TSD is installed globally on the system and this extension
can use it.

### Typings package manager
You must install Typings using npm in order for the Typings package manager
to work.

Open a console and type the following command:

```cmd
npm install typings -g
```

Now Typings is installed globally on the system and this extension can use it.

> Note: TSD is deprecated in favor of Typings.

### bower.json / package.json

You can install packages without having set up Bower, JSPM or npm.

This extension will automatically create the JSON configuration
files, so you don't have to worry about it.

### Customize arguments
Specifying what parameters to use for each of the package managers
can be done in the options page;

![Options](art/options.png)

This give you complete control over how each package is installed.

### Keyboard shortcut

The fastest way to display the dialog is to use the keyboard
shortcut `Shift+Alt+0`.