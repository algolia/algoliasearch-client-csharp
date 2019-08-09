/*
* Copyright (c) 2019 Algolia
* http://www.algolia.com/
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using Newtonsoft.Json;

/// <summary>
/// Alternatives make the rule to trigger on synonyms, typos and plurals.
/// This class is an abstract factory because Alternatives will become in the future a complex JSON Object
/// </summary>
public abstract class Alternatives
{
    /// <summary>
    /// Creates the boolean subtype of Alternatives
    /// </summary>
    /// <param name="value"></param>
    public static Alternatives Of(bool value)
    {
        return new AlternativesBoolean(value);
    }

    /// <summary>
    /// Alternatives can have multiple subtype such as <see cref="AlternativesBoolean"/>
    /// </summary>
    [JsonIgnore]
    public object InsideValue { get; protected set; }
}

internal class AlternativesBoolean : Alternatives
{
    public AlternativesBoolean(bool value)
    {
        InsideValue = value;
    }
}
