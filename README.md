# CalbucciLib.ExtensionsGalore

ExtensionsGalore is a library that extends many of the common types and classes of .NET to provide quick and easy access to common scenarios of web and mobile development. In other words, it helps you write fewer lines of code and focus more in the application.

## Awesomeness in a box

- Comprehensive DeepCopy functionality
- A billion extensions for DateTime for those that have a hard time with dates like me.
- Encode / decode string in many formats (CSV, Tab, Json, Textarea, etc) 
- Language stuff: char.IsVowel/char.IsConsonant, Transliteration, Glyph Mapping, Remove Accents, etc.
- Number: Literal strings, Roman numbers, Pluralization, etc.
- Olson Time Zone extensions to TimeZoneInfo
- Validate: Email, Url, Twitter screenname, Phone Number, Date, Time, etc.

## NuGet

[https://www.nuget.org/packages/CalbucciLib.ExtensionsGalore/](https://www.nuget.org/packages/CalbucciLib.ExtensionsGalore/)



## Principles
- Make the common case easy to use.
- Uses common sense for "unexpected" cases (e.g., treats "null" as empty when possible)
- In a few cases it uses the English language and it has been optimized for EN-US.
- Assumes that parsing & validations are from user input, meaning doesn't be too strict about what's supported dealing properly with whitespaces and variants.

## Best Practices
A lot of the code in this library common from popular answers in StackOverflow or common implementations -- neatly package here, but the vast majority is my own code which I've accumulated over the last 10 years. Wherever approprivate, I left links with the references for the implementation, or the inspiration for the implementation or for the source of information that led to that implementation.


## Extensions Documentation
The best explanation is the source code :). I've added summary meta-data to most methods. But here is a quick overview of the methods available:

----
### Byte[]
* **FromBase62/ToBase62**: Convert a byte[] to/from a Base 62 encoded string (base 62 is better for content in the URL since it doesn't include + / or =).
* **FromBase64/ToBase64**: Just for consistency since it calls Convert.ToBase64String/FromBase64String
* **FromHexEncoding/ToHexEncoding**: Convert a byte[] to/from a hex encoded string.
* **IsEqual**: Compare if two byte[] are identical in content.

----
### Byte
* **FromHex/ToHex**: Convert a single byte from/to hex encoded value.
* **CountBits**: Return the number of bits

----
### Char
* **IsVowel**/**IsConsonant**: Indicates if it's a vowel of consonant, supports accents and most languages.
* **IsASCIILetter**: A-Z or a-z
* **IsASCIILetterOrDigit**: A-Z, a-z or 0-9
* **RemoveAccent**: Returns the same version of the letter without the accent (ä => a, ñ => n)
* **Transliterate**: Returns the latin alphabet version of a letter. This is not a great implementation and needs work.
* **GlyphMap**: Convert characters to the closest latin alphabet visual representation.
* **GlyphMapAndTransliterate**: Convert characters to the closest latin alphabet visual representation and transliterate them.


----
### Color
* **ToColor**: Converts a string to a Color object (supports named colors, HTML/CSS syntaxes)
* **IsAccessibilityContrast**: Compare the contrast between two colors and validates it passes WCAG standard.
* **GetContrastRatio**: Get the contrast ratio between two colors. 0 means no contrast (black-on-black), 21 means maximum contrast (black-on-white)
* **IsColorBlindAccessible**: Not implemented yet.
* **FromHSL**: Create a Color object using HSL.
* **ToHsl**: Returns the HSL components of a color.
* **ToHexColor/ToHtmlColor**: Returns the hex version of a color (e.g. "fecdcd" and "#fecdcd" respectively)
* **ChangeSaturation/ChangeBrightness**: Not implemented yet.

----
### DateTime
* **ParseTime**: Parse a time-only string in any language (e.g. "12p", "17:54", "03:23 A.M.")
* **FromUnixTime/ToUnixTime**: Converts to a Unix-time number
* **ToRelativeTime**: Written version of TimeSpan (e.g. "in 3 minutes" or "7 days ago")
* **IsBetween**: Check if a date is between two other dates
* **CompareTo**: Compare DateTime up to a desired precision (Year, Month, Day, Hour, Minute, Seconds).
* **ElapsedToNow**: Returns the TimeSpan between the date and now (always a positive TimeSpan)
* *Day of month*:
  * **GetFirstDayOfMonth**: Returns the first day of a month (supports a DayOfWeek)
  * **GetFirstDayOfPreviousMonth**: Returns the first day of the previous month
  * **GetFirstDayOfNextMonth**: Returns first day of next month
  * **GetFirstMondayOfMonth**: Returns the first Monday of a month
  * **GetFirstSundayOfMonth**: Returns the first Sunday of a month
  * **GetLastDayOfMonth**: Returns the last day of the month (supports a DayOfWeek)
* *Day of quarter*:
  * **GetFirstDayOfQuarter**: Returns the first day of the quarter (supports a DayOfWeek)
  * **GetLastDayOfQuarter**: Returns the last day of the quarter (supports a DayOfWeek)
* *Previous / Next*:
  * **GetPrevious**: Returns the previous DayOfWeek. If it's the same DayOfWeek as this date, then it returns one week ago.
  * **GetPreviousSunday/GetPreviousMonday**: Returns the previous Sunday/Monday of the calendar.
  * **GetNext**: Returns the next DayOfWeek. If it's the same DayOfWeek as this date, then it returns one week ahead.
  * **GetNextSunday/GetNextMonday**: Returns the next Sunday or Monday.
* **GetAgeInYears**: Return the age in years, ignoring the time component.
* **GetWeekOfYear**: Return the week number of the year.
* *Round / Truncate*:
  * **Round**: Round the date to a specific significance (year, month, day, hour, minute or second)
  * **Truncate**: Truncate the date to a specific significance (year, month, day, hour, minute or second)


----
### Int
* **FromHex/ToHex**: Converts from/to Hex-encoded string.
* **CountBits**: Returns the number of bits.
* **ToRomanNumeral**: Returns the roman numeral.
* **ToRoundedMemorySize**: Returns string representing memory syntax (1024 = "1k", 1048576 = "1M")
* **Pluralize**: Returns a string using the singular or plural version of the words (Compatible with just a few languages).
* **ToLiteral**: Returns a string in English for this number (1025 => "One thousand twenty-five").

----
### List, List&lt;T&gt;
* **ToCSVLine&lt;T&gt;**: Converts a list to a comma-delimited line.
* **ToTabSeparatedLine&gt;T&gt;**: Converts a list to a tab-delimited line.
* **Randomize**: Return a randomized version of the list
* **DeepCopy**: Create a deep-copy of the list (and recursively deep-copies the items)
* **GetPagination**: Returns a "page" of items from the list.
* **GetRandomItem**: Get a single random item from the list.
* **IsEqualUnordered**: Compare two lists treating them as sets (i.e., ignore the order)
* **IsEqualOrdered**: Compare two lists for identical items and order.


----
### Long
* **ToBase62/FromBase62**: Convert to/from a Base 62 encoding
* **CountBits**: Returns the number of bits.
* **ToRoundMemorySize**: Returns string representing memory syntax (1024 = "1k", 1048576 = "1M")
* **ToLiteral**: Returns a string in English for this number (1025 => "One thousand twenty-five").

----
### Object
* **DeepCopy**: Creates a deep-copy of this object, recursively copying every child object as well. Supports all types from System.Collections, System.Collections.Generic and System.Collections.Concurrent. Works with custom classes and anonymous types as well.

----
### Short
* **CountBits**: Returns the number of bits.

----
### String
* **GenerateLoremIpsum**: Generate N words of lorem-ipsum.
* **CreateTRTD**: Converts an array of strings into a TR-TD HTML line
* **HtmlEncode**: Encode a string to be HTML-safe.
* **HtmlEncodeTextArea**: Encode a string to be safely used inside a TEXTAREA element.
* **HtmlDecode**: Decode an HTML-encoded string.
* **UrlEncode/UrlDecode**: Encode/Decode a string to be used in a URL.
* *Escape / Unescape*: 
 * **EscapeCString/UnescapeCString**: Make a string into a literal string to be represented in C/C++/C#/Java(?)
 * **EscapeJson**: Escape a string to be used inside a JSON string.
 * **EscapeStringFormat**: Escape a string to be used in .NET String.Format.
 * **EscapeCSV**: Escape a string to be used as a CSV field.
 * **EscapeCDATA**:  Escape a string to be used in an XML CDATA section.
 * **EscapeTabDelimited**: Escape a string to be used as a field in a Tab-delimited file.
 * **UnescapeCSVField**: Unescape a CSV-escaped string.
 * **UnescapeTabField**: Unescape a Tab-delimited-escaped string.
* *Content Tests*:
 * **HasLowerCase/HasUpperCase**: Indicates if the string has at least one lower or upper case letter.
 * **ContainsAny**: Indicates if the string contains at least one the strings/characters passed as parameters.
 * **StartsWithAny/EndsWithAny**: Indicates if the string starts/ends with at least one of the strings/characters passed as parameters.
 * **CompareNonWhitespace**: Compare the content of a string ignoring the whitespaces ("M 27\t z" == "M27z")
 * **IndexOf(Func)/LastIndexOf(Func)**: Finds the first/last index of a character that matches the predicate.
 * **IndexOfWhitespace/LastIndexOfWhitespace**: Finds the first/last whitepsace/control in a string.
 * **IndexofNonWhitespace/LastIndexOfNonWhitespace**: Finds the first/last non-whitespace/non-control in a string.
 * **IndexOfLetterOrDigit/LastIndexOfLetterOrDigit**: Finds the first/last letter or digit in a string.
 * **IndexOfNonLetterOrDigit/LastIndexOfNonLetterOrDigit**: Finds the first/last non-letter and non-digit in a string.
* *Trim*:
 * **Trim(Func)/TrimStart(Func)/TrimEnd(Func)**: Trim a string using a predicate function.
 * **TrimLower**: Trim a converts to lower case.
 * **TrimInBetween**: Trim leading & trailing whitespaces, and trim consecutive whitespaces into a single space character.
* *Truncate*:
 * **Truncate**: Truncate a string a max-length and trims it.
 * **TruncateEllipsis**: Truncate a string and append ellipsis.
 * **TruncatePhrase**: Truncate a string in word boundary (if possible) and append ellipsis.
 * **TruncateTrimLink**: Truncate and trim a link for display purposes ("http://www.twitter.com/calbucci?#" => "twitter.com/calbucci")
* **RemoveAccents**: Replace accented characters with their non-accented versions ("ação" => "acao")
* **Transliterate**: (Not a good implementation) Converts non-Latin characters (e.g. Hebrew, Arabic, Greek) into Latin characters.
* **GlyphMap**: Convert characters to the closest latin alphabet visual representation.
* **GlyphMapAndTransliterate**: Convert characters to the closest latin alphabet visual representation and transliterate them.
* **CapitalizeFirstWord**: Capitalize the first word of a sentence.
* **CapitalizeAllWords**: Capitalize all words in a sentence.
* **GetFirstWord**: Returns the first word of a sentence.
* **GetLastWord**: Returns the last word of a sentence.
* *Conversion*:
 * **ToBool**: Converts "1", "true", "yes", etc. to true, otherwise false.
 * **ToColor**: Converts any hex-color or HTML color string into a Color object.
 * **ToBytesFromBase64**: Converts a base 64 string into a byte array.
 * **ToBytesFromBase62**: Converts a base 62 string into a byte array.
 * **ToBytesFromHex**: Converts a hex-encoded string into a byte array.
 * **ToInt/ToLong/ToDouble/ToDecimal/ToFloat**: Converts the string to int/long/double/decimal/float. A lot more flexible than int.Parse and it returns 0 if it can't parse it.
 * **ToEnum(T)**: Converts the string to an Enum type.
 * **ToListFromCsvLine**: Converts a string that is a line in a CSV file into a list of field values.
 * **ToListFromTabDelimitedLine**: Converts a string that is a line in a Tab-delimited file into a list of field values.

----
### TimeZoneInfo
* **ToOlsonTimeZone**: Returns the best match for the Olson Time Zone format.
* **ToOlsonTimeZones**: Returns all the matches for the Olson Time Zone format.
* **FromOlsonToTimeZoneId**: Returns the TimeZone Id from Olson Time Zone.
* **FromOlsonToTimeZoneInfo**: Returns the TimeZoneInfo from Olson Time Zone.

----
### UInt
* **CountBits**: Returns the number of bits.
* **ToLiteral**: Returns a string in English for this number (1025 => "One thousand twenty-five").


----
### ULong
* **CountBits**: Returns the number of bits.
* **ToLiteral**: Returns a string in English for this number (1025 => "One thousand twenty-five").

----
### Validate
* **IsValidEmail**: It's a valid email address
* **IsValidLink**: It's value of a link, absolute or relative, for multiple schemes (HTTP, MAILTO, FTP, etc.)
* **IsValidDomain**: It's a valid domain name.
* **IsValidUSPhoneNumber**: It's a valid US phone number.
* **IsValidPhoneNumber**: It's a valid phone number (not a great implementation since it doesn't account for lots of INTL nuances)
* **IsValidBase64String**: It's a valid base 64 string.
* **IsValidHTMLColor**: It's a valid HTML/CSS color
* **IsValidTwitterUsername**: It's a valid Twitter Username
* **IsValidMailToAddress**:  It's a valid mailto: address
* **IsValidDomainTLD**: It's a valid Top-Level Domain
* **IsValidGuid**: It's a valid Guid.
* **IsValidIPv4**: It's a valid IP v.4.
* **IsValidIPv6**: It's a valid IP v.6.
* **IsValidIP**:  It's a valid IP Address (v.4 or v.6)
* **IsValidTime**: It's a valid time string (any language)
* **IsValidDate**: It's a valid date







----
## Contributors

- ExtensionsGalore was originally created by *Marcelo Calbucci* ([blog.calbucci.com](http://blog.calbucci.com) | [@calbucci](http://twitter.com/calbucci))