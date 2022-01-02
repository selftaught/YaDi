# YADI (Yet Another DLL Injector)

YaDi is a Windows DLL injector that provides different techniques of injection. YADI is written in C# and leverages functions exported by the Windows API to interact with processes / achieve code injection and execution.

## Install

### From source

### From binary

## Injection Methods

- LoadLibrary
- SetWindowsHookEx
- Thread Hijack (AKA Suspend, Inject, Resume (S.I.R.))
- QueueUserAPC
- IAT Hook

---

## TODO

 - [ ] Add x86 support
 - [ ] Add unit tests
 - [ ] Implement SetWindowsHookEx DLL injection
 - [ ] Implement APC DLL injection
 - [ ] Implement IAT Hook DLL injection
 - [ ] Implement Reflective DLL injection
 - [ ] Create a command line utility for injecting

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
