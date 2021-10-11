## DotNetPerf 

Eksempel-applikasjon for prosess rundt ytelsesoptimalisering i .NET.
Som eksempel brukes kalkulering av outrights-markeder i fotball.
Et API endepunkt tar inn en liste med lag og "expected goals", og simulerer en sesong
N ganger.

F�rste utkast av applikasjonen er i `initial`-branch.
Med utgangspunkt i denne kj�rer jeg profiling og benchmarking og implementerer forskjellige optimaliseringer i PRs.

Se [benchmark resultater her](/tests/DotNetPerf.Benchmarks/Outrights).

### Tools

* [benchmarkdotnet](https://benchmarkdotnet.org/)
  * Also has [DisassemblyDiagnoser](https://benchmarkdotnet.org/articles/features/disassembler.html)
* [PerfView](https://github.com/microsoft/perfview)
* [dotTrace](https://www.jetbrains.com/profiler/), [dotMemory](https://www.jetbrains.com/dotmemory/)
* [Visual Studio Profiler](https://docs.microsoft.com/en-us/visualstudio/profiling/?view=vs-2022)
* Distributed tracing
  * [opentelemetry](https://opentelemetry.io/)
  * [Jaeger](https://www.jaegertracing.io/)
* [ObjectLayoutInspector](https://github.com/SergeyTeplyakov/ObjectLayoutInspector) for � se st�rrelsen p� en class/struct i en gitt runtime
* [Sharplab](https://sharplab.io/) for � se p� codegen - IL og JIT asm
* [ILSpy](https://github.com/icsharpcode/ILSpy)

### Ressurser

* [awesome-dot-net-performance](https://github.com/adamsitnik/awesome-dot-net-performance)
* [Dotnetos p� YouTube](https://www.youtube.com/c/Dotnetos)

### Data innhenting

https://no.wikipedia.org/wiki/Premier_League_2020/21

```js
// temp1 = tbody lagret som global variabel i console
// Deretter kopiert inn i testinput.json
Array.prototype.map.call(Array.prototype.slice.call(temp1.children, 1), n => ({ name: n.children[1].innerText, expectedGoals: parseInt(n.children[6].innerText, 10) / 38.0}))
```