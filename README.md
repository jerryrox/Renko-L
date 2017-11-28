### A huuuuge work-in-progress

# Renko-L
My own small library for future (and current) Unity app development.
I might as well use this chance to study how to use GitHub :p

Unity Version: 5.6.4

### Few things to note
- Some features may not work or require modification due to the difference in the API.

## Namespaces

### Renko
Currently, nothing is included in this namespace

### Renko.Data
Provides modules that deal with user data management (ex. database or playerprefs) are included.

### Renko.Debug
Provides a single class and enum for logging information to the console.
You may use the logger in this namespace if you want an easy way to toggle log outputs for specific log-level or completely disable it.

### Renko.Extensions
Provides various extensions that may come in handy.

### Renko.Network
Provides modules related to networking for quick, easy way of implementing network features.

### Renko.Plugin
Provides interfaces for handling platform-specific actions.
Examples:
- Opening camera app on Android/iOS
- Handling push messages

### Renko.Security
Provides an interface for simple encryption / decryption process.

### Renko.Services
Provides wrappers for Unity service modules.
You can easily get rid of the modules that you don't use in your project without any extra changes.

### Renko.Threading
Provides modules for handling multi-threaded processes.

### Renko.UIFramework
A simple framework using NGUI.

### Renko.Utility
Provides various scripts / modules that makes my coding easier.

## Scripting Def Symbols
- NGUI
