# FinderSeeker

A fast file and content search for Windows. It reads the file system directly, in parallel, and does not use Windows Search
or its index, so the results are always current and nothing has to be indexed first.

## Searching

| Criteria | |
| --- | --- |
| **Look in** | One or more folders or drives (separate with `;`), or *All local drives*. Folders you have used are remembered. |
| **Name** | Matched as **wildcards** (`*.cs;*.vb`, a name with no wildcard finds names containing it), **plain text**, or a **regular expression**. Optional match case, and optionally against the full path. |
| **Contains** | Text to find *inside* files, as **plain text**, a **wildcard pattern** or a **regular expression**. Optional match case. |
| **Type** | Any, text, source code, images, documents, audio, video, archives or programs. |
| **Show** | Files, folders, or both. |
| **Exclude** | Names to skip, e.g. `node_modules;.git;*.tmp`. Excluded folders are not entered. |
| **Size** | At least / at most, in bytes, KB, MB or GB. |
| **Dates** | Modified and created, after and/or before. |
| **Other** | Include hidden / system items, search binary files, skip text files over a size. |

Results appear while the search runs and can be sorted by any column. For a content search the *Match* column shows the line
number and the text around the first match.

Double-click (or Enter) opens a result; the right-click menu opens its folder in Explorer and copies paths.

### Content search notes

- Plain text is found with vectorized searches over large blocks of the file, and can contain a line break.
- Wildcard and regular expression patterns are matched one line at a time (`.` and `\s` do not cross a line break; `^` and `$` mean
  the start and end of a line). A pattern that takes more than two seconds on a file skips that file.
- Files are read as UTF-8, or UTF-8/16/32 when they start with a byte order mark. A file that has zero bytes in its first 4 KB and
  no byte order mark is treated as binary and skipped, unless *Search binary files* is ticked (then it is read as single-byte text).
- Files that are open in other programs can still be read.

## Preview

The panel on the right (View > Preview panel, `F11`) shows the selected result: images (bmp, png, jpg, gif, tif, ico, emf, wmf)
as a picture, text files as text with the content search's matches highlighted, other files as a hex dump of their beginning,
and folders as a count of what is inside.

## Keyboard

| Key | |
| --- | --- |
| `Enter` / `F5` | Search |
| `Esc` | Stop |
| `Ctrl+F` / `Ctrl+Shift+F` | Go to *Name* / *Contains* |
| `Ctrl+A`, `Ctrl+C` | Select all results, copy their paths |
| `F11` | Show or hide the preview |

## Theme

The app follows the Windows light/dark setting (restart it after changing the Windows theme).

## Languages

The app uses the Windows display language. To use another, choose **File > Language...**; it applies at once and is remembered
(the `"Language"` setting, a culture name such as `de` or `pt-BR`; empty follows Windows). Text with no translation is shown in English.

This works the same way as in SQL Script Generator. Translations are in a SQLite file, `FinderSeeker/Translations/Translations.db`,
copied next to the program. The table `Translation` has one row for each English phrase and one column for each language; the
English text is the key. Text that the Windows Forms designer puts on a form is translated automatically when the form is created (`FormLocalizer`), so designer text needs nothing in the code. In code, wrap text for people in `Localize`:

```csharp
Text = Localize("Search");
MessageBox.Show(Localize("Folder {0} does not exist.", folder));
```


To add new phrases to the database and translate the empty cells, run `LocalizerScan.bat` (it runs the `LocalizerScan` project in this repository and needs the Einkr key in `C:\EinkrKey.txt`).

## Where things are kept

| What | Where |
| --- | --- |
| Settings and history | `%AppData%\NTDLS\FinderSeeker\settings.json` |
| Logs | `%LocalAppData%\NTDLS\FinderSeeker\Logs` (Help > Open log folder) |

## Building

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download). The solution is `FinderSeeker.slnx`.

```
dotnet build FinderSeeker.slnx
dotnet test Tests/Tests.csproj
```

To build the installer (needs [Inno Setup 6](https://jrsoftware.org/isinfo.php)), publish self-contained and then compile
`Installer/Installer.iss`:

```
dotnet publish FinderSeeker/FinderSeeker.csproj -c Release -r win-x64 --self-contained
```
