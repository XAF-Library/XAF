using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// In Projekten im SDK-Stil wie dem vorliegenden, bei dem verschiedene Assemblyattribute
// üblicherweise in dieser Datei definiert wurden, werden diese Attribute jetzt während
// der Builderstellung automatisch hinzugefügt und mit Werten aufgefüllt, die in den
// Projekteigenschaften definiert sind. Informationen dazu, welche Attribute einbezogen
// werden und wie dieser Prozess angepasst werden kann, finden Sie unter https://aka.ms/assembly-info-properties.


// Wenn "ComVisible" auf FALSE festgelegt wird, sind die Typen in dieser Assembly
// für COM-Komponenten nicht sichtbar. Wenn Sie von COM aus auf einen Typ in dieser
// Assembly zugreifen müssen, legen Sie das ComVisible-Attribut für den betreffenden
// Typ auf TRUE fest.

[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM
// bereitgestellt wird.

[assembly: Guid("f16d6c96-de35-496f-9fd1-172093dbae9b")]

[assembly: InternalsVisibleTo("XAF.UI.WPF")]
