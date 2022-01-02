# YADI (Yet Another DLL Injector)

---

YaDi is a Windows x86/x64 DLL injector. YADI leverages P/Invoke to make native windows calls exported by kernel32.dll, user32.dll, and psapi.dll to achieve code injection. I started this project to learn some C# and to become more familiar with various injection techiniques.

## Usage

To use YaDi, download the appropriate release and run it. Browse for a DLL file, select a process from the list view, and click inject.

## Features


- Various injection methods
    - LoadLibrary
    - SetWindowsHookEx
    - Thread Hijack (AKA Suspend, Inject, Resume (S.I.R.))
    - QueueUserAPC
    - IAT Hook
- x86 support
- x64 support



## Contributing

1. Fork it
2. Create feature branch (`git checkout -b feature/feature-desc`)
3. Make some changes...
4. Commit changes (`git commit -m '...'`)
5. Push changes to remote feature branch (`git push origin feature/feature-desc`)
6. Create a pull request when the feature branch is ready for review

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
