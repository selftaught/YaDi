# YaDi (Yet another DLL injector)

---

YaDi is a Windows x86/x64 DLL injector. YaDi leverages P/Invoke to make native windows calls exported by kernel32.dll, user32.dll, and psapi.dll to achieve code injection. I started this project to learn some C# and to become more familiar with various injection techiniques.

![YaDi](https://i.imgur.com/bBcGIEA.png)

## Usage

To use YaDi, download the appropriate release and run it. Browse for a DLL file, select a process from the list view, and click inject.

## Features

- LoadLibrary/CreateRemoteThread injection
- x86 support
- x64 support

## TODO

 - [ ] Improve error handling
 - [ ] Logging to filesystem
 - [ ] Toolbar submenu forms
 - [ ] SetWindowsHookEx injection
 - [ ] QueueUserAPC injection
 - [ ] IAT hook injection


## Contributing

1. Fork it
2. Create a feature / bugfix branch
3. Make some changes
4. Commit changes
5. Push changes to remote feature branch
6. Create a pull request

## License

> The MIT License (MIT)
>
> Copyright (C) 2021 by Dillan Hildebrand
>
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
>
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
>
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
