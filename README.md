# YADI (Yet Another DLL Injector)

YADI is a Windows DLL injector that provides different techniques of injection. YADI is written in C# and leverages functions exported by the Windows API to interact with processes / achieve code injection and execution.

## Install

### From source

### From binary

## Usage

### GUI

1. Run yadi-gui.exe
2. Select DLL path
3. Select DLL injection method
4. Select target process from process list
5. Click Inject

### CLI

`yadi.exe <pid> <dllpath> <injectionmethod>`

## Injection Methods

### LoadLibrary + CreateRemoteThread

---

Loads a specified module into the address space of the calling process. The specified module may cause other modules to be loaded. The DLL is loaded into the process address space through a call to LoadLibrary. Then the DLL code is invoked by making a call to CreateRemoteThread.

**PROS**

- Easy implementation

**CONS**

- Easy detection

### SetWindowsHookEx
0
---

Installs an application-defined hook procedure into a hook chain which is then invoked whenever certain events are triggered.

**PROS**

**CONS**

### Thread Hijack (AKA Suspend, Inject, Resume (S.I.R.))

---

Thread Hijack DLL injection works by writing code into the target process' memory, suspending a thread in that target process, injecting code, setting the paused thread's instruction pointer to the address of that code, and then finally resuming the thread which results in the execution of the injected DLL code.

**PROS**

**CONS**

### Reflective

---

`@TODO`

**PROS**

**CONS**

### QueueUserAPC

---

QueueUserAPC (Queue User Asynchronous Procedure Call)

`@TODO`

**PROS**

**CONS**

### IAT Hooking

---

IAT (Import Address Table) hooking

`@TODO`

**PROS**

**CONS**

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