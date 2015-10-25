using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.ExtensionsGalore
{
	public enum DatePartsOrder
	{
		/// <summary>
		/// Year/Month/Day
		/// </summary>
		YMD,

		/// <summary>
		/// Day/Month/Year
		/// </summary>
		DMY,

		/// <summary>
		/// Month/Day/Year
		/// </summary>
		MDY
	}
	static public class Validate
	{
		private static HashSet<string> _TLDs;

		static private string _ValidPhoneNumberSymbols = " .-()#_+";
		static private Dictionary<string, string> _USAreaCodes;
        private static HashSet<string> _NotValidTwitterAccounts;


        static Validate()
		{
            _NotValidTwitterAccounts = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                "about",
                "favorites",
                "followers",
                "following",
                "hashtag",
                "i",
                "intent",
                "messages",
                "privacy",
                "search",
                "search-advanced",
                "search-home",
                "settings",
                "signin",
                "signup",
                "tos",
                "who_to_follow",
            };

            // http://www.bennetyee.org/ucsd-pages/area.html
            string[] allAreaCodesZone1 = new string[]
			{
				"201","202","203","204","205","206","207","208","209","210","211","212","213","214","215","216","217","218","219","224","225","226","228","229","231","234","236","239","240","242","246","248",
				"250","251","252","253","254","256","260","262","264","267","268","269","270","276","278","281","283","284","289",
				"301","302","303","304","305","306","307","308","309","310","311","312","313","314","315","316","317","318","319","320","321","323","325","330","331","334","336","337","339","340","341","345","347",
				"351","352","360","361","369","380","385","386","401","402","403","404","405","406","407","408","409","410","411","412","413","414","415","416","417","418","419","423","424","425","430","432","434","435","438","440","441","442","443",
				"450","456","464","469","470","473","475","478","479","480","484",
				"500","501","502","503","504","505","506","507","508","509","510","511","512","513","514","515","516","517","518","519","520","530","540","541",
				"551","555","557","559","561","562","563","564","567","570","571","573","574","575","580","585","586",
				"600","601","602","603","604","605","606","607","608","609","610","611","612","613","614","615","616","617","618","619","620","623","626","627","628","630","631","636","641","646","647","649",
				"650","651","660","661","662","664","669","670","671","678","679","682","684","689",
				"700","701","702","703","704","705","706","707","708","709","710","711","712","713","714","715","716","717","718","719","720","724","727","731","732","734","737","740","747",
				"754","757","758","760","762","763","764","765","767","769","770","772","773","774","775","778","779","780","781","784","785","786","787",
				"800","801","802","803","804","805","806","807","808","809","810","811","812","813","814","815","816","817","818","819","822","828","829","830","831","832","833","835","843","844","845","847","848",
				"850","855","856","857","858","859","860","862","863","864","865","866","867","868","869","870","872","876","877","878","880","881","882","888","898",
				"900","901","902","903","904","905","906","907","908","909","910","911","912","913","914","915","916","917","918","919","920","925","927","928","931","935","936","937","939","940","941","947","949",
				"951","952","954","956","957","959","970","971","972","973","975","976","978","979","980","984","985","989"			
			};
			_USAreaCodes = new Dictionary<string, string>(allAreaCodesZone1.Length);
			foreach (var ac in allAreaCodesZone1)
				_USAreaCodes[ac] = ac;

			// Updated on 5/13/2015 from http://en.wikipedia.org/wiki/List_of_Internet_top-level_domains
			string[] tlds =
			{
				"com", "net", "org", "mil", "gov", "edu",

				"aero", "arpa", "biz", "coop", "info", "int", "museums", "name", "pro", "tel", "xxx", "mobi", "me",

				"academy", "accountants", "active", "actor", "adult", "aero", "agency", "airforce", "app", "archi", "army",
				"associates", "attorney", "auction", "audio", "autos", "band", "bar", "bargains", "beer", "best", "bid", "bike",
				"bio", "biz", "black", "blackfriday", "blog", "blue", "boo", "boutique", "build", "builders", "business", "buzz",
				"cab", "camera", "camp", "cancerresearch", "capital", "cards", "care", "career", "careers", "cash", "catering",
				"center", "ceo", "channel", "cheap", "christmas", "church", "city", "claims", "cleaning", "click", "clinic",
				"clothing", "club", "coach", "codes", "coffee", "college", "community", "company", "computer", "condos",
				"construction", "consulting", "contractors", "cooking", "cool", "country", "credit", "creditcard", "cricket",
				"cruises", "dad", "dance", "dating", "day", "deals", "degree", "delivery", "democrat", "dental", "dentist",
				"diamonds", "diet", "digital", "direct", "directory", "discount", "domains", "eat", "education", "email", "energy",
				"engineer", "engineering", "equipment", "esq", "estate", "events", "exchange", "expert", "exposed", "fail", "farm",
				"fashion", "feedback", "finance", "financial", "fish", "fishing", "fit", "fitness", "flights", "florist", "flowers",
				"fly", "foo", "forsale", "foundation", "fund", "furniture", "gallery", "garden", "gift", "gifts", "gives", "glass",
				"global", "gop", "graphics", "green", "gripe", "guide", "guitars", "guru", "healthcare", "help", "here", "hiphop",
				"hiv", "holdings", "holiday", "homes", "horse", "host", "hosting", "house", "how", "info", "ing", "ink",
				"institute", "insure", "international", "investments", "jobs", "kim", "kitchen", "land", "lawyer", "legal",
				"lease", "lgbt", "life", "lighting", "limited", "limo", "link", "loans", "lotto", "luxe", "luxury", "management",
				"market", "marketing", "media", "meet", "meme", "memorial", "menu", "mobi", "moe", "money", "mortgage",
				"motorcycles", "mov", "museum", "name", "navy", "network", "new", "ngo", "ninja", "ong", "onl", "ooo",
				"organic", "partners", "parts", "party", "pharmacy", "photo", "photography", "photos", "physio", "pics", "pictures",
				"pink", "pizza", "place", "plumbing", "poker", "porn", "post", "press", "pro", "productions", "prof", "properties",
				"property", "qpon", "recipes", "red", "rehab", "ren", "rentals", "repair", "report", "republican", "rest",
				"reviews", "rich", "rip", "rocks", "rodeo", "rsvp", "sale", "science", "services", "sexy", "shoes", "singles",
				"social", "software", "solar", "solutions", "space", "supplies", "supply", "support", "surf", "surgery", "systems",
				"tattoo", "tax", "technology", "tel", "tips", "tires", "today", "tools", "top", "town", "toys", "trade", "training",
				"travel", "university", "vacations", "vet", "video", "villas", "vision", "vodka", "vote", "voting", "voyage", "wang",
				"watch", "webcam", "website", "wed", "wedding", "whoswho", "wiki", "work", "works", "world", "wtf",
				"xxx", "xyz", "yoga"
			};

			string[] ctlds =
			{
				"ac", // Ascension Island
				"ad", // Andorra
				"ae", // United Arab Emirates
				"af", // Afghanistan
				"ag", // Antigua and Barbuda
				"ai", // Anguilla
				"al", // Albania
				"am", // Armenia
				"an", // Netherlands Antilles
				"ao", // Angola
				"aq", // Antarctica
				"ar", // Argentina
				"as", // American Samoa
				"at", // Austria
				"au", // Australia
				"aw", // Aruba
				"ax", // Aland Islands
				"az", // Azerbaijan
				"ba", // Bosnia and Herzegovina
				"bb", // Barbados
				"bd", // Bangladesh
				"be", // Belgium
				"bf", // Burkina Faso
				"bg", // Bulgaria
				"bh", // Bahrain
				"bi", // Burundi
				"bj", // Benin
				"bm", // Bermuda
				"bn", // Brunei Darussalam
				"bo", // Bolivia
				"br", // Brazil
				"bs", // Bahamas
				"bt", // Bhutan
				"bv", // Bouvet Island
				"bw", // Botswana
				"by", // Belarus
				"bz", // Belize
				"ca", // Canada
				"cc", // Cocos (Keeling) Islands
				"cd", // Congo, The Democratic Republic of the
				"cf", // Central African Republic
				"cg", // Congo, Republic of
				"ch", // Switzerland
				"ci", // Cote d'Ivoire
				"ck", // Cook Islands
				"cl", // Chile
				"cm", // Cameroon
				"cn", // China
				"co", // Colombia
				"cr", // Costa Rica
				"cs", // Serbia and Montenegro
				"cu", // Cuba
				"cv", // Cape Verde
				"cx", // Christmas Island
				"cy", // Cyprus
				"cz", // Czech Republic
				"de", // Germany
				"dj", // Djibouti
				"dk", // Denmark
				"dm", // Dominica
				"do", // Dominican Republic
				"dz", // Algeria
				"ec", // Ecuador
				"ee", // Estonia
				"eg", // Egypt
				"eh", // Western Sahara
				"er", // Eritrea
				"es", // Spain
				"et", // Ethiopia
				"eu", // European Union
				"fi", // Finland
				"fj", // Fiji
				"fk", // Falkland Islands (Malvinas)
				"fm", // Micronesia, Federal State of
				"fo", // Faroe Islands
				"fr", // France
				"ga", // Gabon
				"gb", // United Kingdom
				"gd", // Grenada
				"ge", // Georgia
				"gf", // French Guiana
				"gg", // Guernsey
				"gh", // Ghana
				"gi", // Gibraltar
				"gl", // Greenland
				"gm", // Gambia
				"gn", // Guinea
				"gp", // Guadeloupe
				"gq", // Equatorial Guinea
				"gr", // Greece
				"gs", // South Georgia and the South Sandwich Islands
				"gt", // Guatemala
				"gu", // Guam
				"gw", // Guinea-Bissau
				"gy", // Guyana
				"hk", // Hong Kong
				"hm", // Heard and McDonald Islands
				"hn", // Honduras
				"hr", // Croatia/Hrvatska
				"ht", // Haiti
				"hu", // Hungary
				"id", // Indonesia
				"ie", // Ireland
				"il", // Israel
				"im", // Isle of Man
				"in", // India
				"io", // British Indian Ocean Territory
				"iq", // Iraq
				"ir", // Iran, Islamic Republic of
				"is", // Iceland
				"it", // Italy
				"je", // Jersey
				"jm", // Jamaica
				"jo", // Jordan
				"jp", // Japan
				"ke", // Kenya
				"kg", // Kyrgyzstan
				"kh", // Cambodia
				"ki", // Kiribati
				"km", // Comoros
				"kn", // Saint Kitts and Nevis
				"kp", // Korea, Democratic People's Republic
				"kr", // Korea, Republic of
				"kw", // Kuwait
				"ky", // Cayman Islands
				"kz", // Kazakhstan
				"la", // Lao People's Democratic Republic
				"lb", // Lebanon
				"lc", // Saint Lucia
				"li", // Liechtenstein
				"lk", // Sri Lanka
				"lr", // Liberia
				"ls", // Lesotho
				"lt", // Lithuania
				"lu", // Luxembourg
				"lv", // Latvia
				"ly", // Libyan Arab Jamahiriya
				"ma", // Morocco
				"mc", // Monaco
				"md", // Moldova, Republic of
				"mg", // Madagascar
				"mh", // Marshall Islands
				"mk", // Macedonia, The Former Yugoslav Republic of
				"ml", // Mali
				"mm", // Myanmar
				"mn", // Mongolia
				"mo", // Macau
				"mp", // Northern Mariana Islands
				"mq", // Martinique
				"mr", // Mauritania
				"ms", // Montserrat
				"mt", // Malta
				"mu", // Mauritius
				"mv", // Maldives
				"mw", // Malawi
				"mx", // Mexico
				"my", // Malaysia
				"mz", // Mozambique
				"na", // Namibia
				"nc", // New Caledonia
				"ne", // Niger
				"nf", // Norfolk Island
				"ng", // Nigeria
				"ni", // Nicaragua
				"nl", // Netherlands
				"no", // Norway
				"np", // Nepal
				"nr", // Nauru
				"nu", // Niue
				"nz", // New Zealand
				"om", // Oman
				"pa", // Panama
				"pe", // Peru
				"pf", // French Polynesia
				"pg", // Papua New Guinea
				"ph", // Philippines
				"pk", // Pakistan
				"pl", // Poland
				"pm", // Saint Pierre and Miquelon
				"pn", // Pitcairn Island
				"pr", // Puerto Rico
				"ps", // Palestinian Territory, Occupied
				"pt", // Portugal
				"pw", // Palau
				"py", // Paraguay
				"qa", // Qatar
				"re", // Reunion Island
				"ro", // Romania
				"rs", // Serbia
				"ru", // Russian Federation
				"rw", // Rwanda
				"sa", // Saudi Arabia
				"sb", // Solomon Islands
				"sc", // Seychelles
				"sd", // Sudan
				"se", // Sweden
				"sg", // Singapore
				"sh", // Saint Helena
				"si", // Slovenia
				"sj", // Svalbard and Jan Mayen Islands
				"sk", // Slovak Republic
				"sl", // Sierra Leone
				"sm", // San Marino
				"sn", // Senegal
				"so", // Somalia
				"sr", // Suriname
				"st", // Sao Tome and Principe
				"sv", // El Salvador
				"sy", // Syrian Arab Republic
				"sz", // Swaziland
				"tc", // Turks and Caicos Islands
				"td", // Chad
				"tf", // French Southern Territories
				"tg", // Togo
				"th", // Thailand
				"tj", // Tajikistan
				"tk", // Tokelau
				"tl", // Timor-Leste
				"tm", // Turkmenistan
				"tn", // Tunisia
				"to", // Tonga
				"tp", // East Timor
				"tr", // Turkey
				"tt", // Trinidad and Tobago
				"tv", // Tuvalu
				"tw", // Taiwan
				"tz", // Tanzania
				"ua", // Ukraine
				"ug", // Uganda
				"uk", // United Kingdom
				"um", // United States Minor Outlying Islands
				"us", // United States
				"uy", // Uruguay
				"uz", // Uzbekistan
				"va", // Holy See (Vatican City State)
				"vc", // Saint Vincent and the Grenadines
				"ve", // Venezuela
				"vg", // Virgin Islands, British
				"vi", // Virgin Islands, U.S.
				"vn", // Vietnam
				"vu", // Vanuatu
				"wf", // Wallis and Futuna Islands
				"ws", // Western Samoa
				"ye", // Yemen
				"yt", // Mayotte
				"yu", // Yugoslavia
				"za", // South Africa
				"zm", // Zambia
				"zw"  // Zimbabwe
			};			

			_TLDs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
			foreach (var tld in tlds)
				_TLDs.Add(tld);
			foreach (var ctld in ctlds)
				_TLDs.Add(ctld);
		}
		static public bool IsValidEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				return false;

			if (email.Length < 6 || email.Length > 128)
				return false;

			int pos = email.IndexOf('@');
			if (pos <= 0)
				return false;

			string alias = email.Substring(0, pos);
			if(string.IsNullOrWhiteSpace(alias) || alias.Length > 64)
				return false;

			if (alias[0] == '\"')
			{
				// Quoted email alias
				if (alias[alias.Length - 1] != '\"')
					return false;

				alias = alias.Substring(1, alias.Length - 2);
			}

			// must start with letter or digit
			if (!char.IsLetterOrDigit(alias, 0))
				return false;

			// must end in letter or digit or underscore or dash
			char last = alias[alias.Length - 1];
			if (!last.IsASCIILetterOrDigit() && last != '_' && last != '-')
				return false;

			foreach (char c in alias)
			{
				if (c >= 'a' && c <= 'z')
					continue;
				if (c >= 'A' && c <= 'Z')
					continue;
				if (c >= '0' && c <= '9')
					continue;
				if ("-_%.=/+'".IndexOf(c) >= 0)
					continue;
				return false;
			}

			string domain = email.Substring(pos + 1);
			return IsValidDomain(domain, true);
		}

		static public bool IsValidLink(string link, bool acceptRelative = false)
		{
			try
			{
				var uri = new Uri(link);
				if (!uri.IsAbsoluteUri)
					return acceptRelative;

				switch (uri.Scheme.ToLowerInvariant())
				{
					case "http":
					case "https":
					if (!IsValidDomain(uri.Host))
						return false;
						break;
					case "mailto":
						if (!IsValidEmail(uri.UserInfo + "@" + uri.Host))
							return false;
						break;
					case "file":
					case "ftp":
					case "gopher":
					case "ldap":
					case "net.pipe":
					case "net.tcp":
					case "news":
					case "nntp":
					case "telnet":
					case "uuid":
						break;
					default:
						return false;
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		static public bool IsValidDomain(string domainName, bool internetValid = true)
		{
			if (domainName == null || domainName.Length < 4)
				return false;

			if (IsValidIP(domainName))
				return true;

			string[] parts = domainName.Split('.');
			foreach (string part in parts)
			{
				if (string.IsNullOrWhiteSpace(part))
					return false;

				string p2 = part.ToLower();
				if (p2[0] == '-' || p2[p2.Length - 1] == '-')
					return false; // cannot start or end with a dash

				foreach (char c in p2)
				{
					if (c >= 'a' && c <= 'z')
						continue;
					if (c >= '0' && c <= '9')
						continue;
					if (c == '-')
						continue;
					return false;
				}
			}

			if (internetValid)
			{
				if (domainName.Equals("locahost", StringComparison.InvariantCultureIgnoreCase))
					return true;
				if (parts.Length < 2)
					return false; // Must have at least a host and TLD
				string tld = parts[parts.Length - 1];
				return _TLDs.Contains(tld);
			}

			return true;

		}

		static public bool IsValidUSPhoneNumber(string phoneNumber, bool withAreaCode = true)
		{
			if (String.IsNullOrWhiteSpace(phoneNumber))
				return false;

			if (phoneNumber.Length < 7)
				return false;

			StringBuilder allDigits = new StringBuilder();

			int p1 = phoneNumber.IndexOf('+');
			if (p1 >= 0)
			{
				if (p1 == phoneNumber.Length - 1)
					return false;

				string nextCountryDigit = phoneNumber.Substring(p1 + 1, 1);
				if (nextCountryDigit != "1")
					return false;

				phoneNumber = phoneNumber.Substring(p1 + 2);
			}

			foreach (char c in phoneNumber)
			{
				if (Char.IsDigit(c))
					allDigits.Append(c);
				else if (_ValidPhoneNumberSymbols.IndexOf(c) == -1)
					return false;
			}

			string digits = allDigits.ToString();

			if (digits.Length == 10)
			{
				string areaCode = digits.Substring(0, 3);
				if (!_USAreaCodes.ContainsKey(areaCode))
					return false;
				digits = digits.Substring(3);
			}
			else if (withAreaCode)
				return false;

			if (digits.Length != 7)
				return false;


			// cannot start with 0 or 1
			if (digits[0] == '0' || digits[0] == '1')
				return false;

			// cannot start with 555
			if (digits.StartsWith("555"))
				return false;

			return true;
		}

		static public bool IsValidPhoneNumber(string phoneNumber)
		{
			if (string.IsNullOrWhiteSpace(phoneNumber))
				return false;

			if (phoneNumber.Length < 6)
				return false;

			int digitCount = 0;
			foreach (char c in phoneNumber)
			{
				if (char.IsDigit(c))
				{
					digitCount++;
					continue;
				}
				else if (char.IsControl(c) || char.IsWhiteSpace(c))
				{
					continue;
				}
				if (!_ValidPhoneNumberSymbols.Contains(c))
				{
					return false;
				}
			}

			return digitCount >= 6 && digitCount <= 14;
		}

		static public bool IsValidBase64String(string base64String)
		{
			if (string.IsNullOrEmpty(base64String))
				return true;

			foreach (char c in base64String)
			{
				if (c >= 'a' && c <= 'z')
					continue;
				if (c >= 'A' && c <= 'Z')
					continue;
				if (c >= '0' && c <= '9')
					continue;
				if (c == '=' || c == '+' || c == '/')
					continue;
				return false;
			}

			return true;
		}

		static public bool IsValidHtmlColor(string htmlColor)
		{
			var color = ColorExtensions.ToColor(htmlColor);
			return color != null;
		}

		static public bool IsValidTwitterUsername(string twitterUsername)
		{
			if (string.IsNullOrWhiteSpace(twitterUsername))
				return false;

			int start = 0;
			if (twitterUsername.StartsWith("@"))
				start = 1;

			if (twitterUsername.Length - start > 15)
				return false; // max 15 characters

			for (int i = start; i < twitterUsername.Length; i++)
			{
				char c = twitterUsername[i];
				if (c >= 'a' && c <= 'z')
					continue;
				if (c >= 'A' && c <= 'Z')
					continue;
				if (c >= '0' && c <= '9')
					continue;
				if (c == '_')
					continue;
				return false;
			}

		    if (start == 1)
		        twitterUsername = twitterUsername.Substring(1);

		    return !_NotValidTwitterAccounts.Contains(twitterUsername);
		}

		/// <summary>
		/// Validates a the "to" part of the mailto: URI
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		static public bool IsValidMailToAddress(string email)
		{
			int ltp = email.IndexOf('<');
			if (ltp >= 0)
			{
				// Composite email as in "Name <email>"
				if (ltp > 0)
				{
					string name = email.Substring(0, ltp).Trim();
					if (!string.IsNullOrWhiteSpace(name))
					{
						foreach (char c in name)
						{
							if (char.IsControl(c))
								return false;
						}
					}
				}

				int gtp = email.IndexOf('>');
				if (gtp != email.Length - 1)
					return false; // must be the last character

				email = email.Substring(ltp + 1, email.Length - ltp - 2).Trim();
				if (email == null || email.Length < 6)
					return false;
			}

			return IsValidEmail(email);
		}

		static public bool IsValidDomainTLD(string tld)
		{
			if (string.IsNullOrWhiteSpace(tld) || tld.Length < 2)
				return false;

			if (tld[0] == '.')
			{
				tld = tld.Substring(1);
				if (tld.Length < 2)
					return false;
			}

			return _TLDs.Contains(tld);
		}

		/// <summary>
		/// Validates if the string contains a GUID
		/// </summary>
		static public bool IsValidGuid(string guid)
		{
			try
			{
				Guid.Parse(guid);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Validates if the string contains a well-formed dot-quad IP address v4.
		/// </summary>
		/// <param name="ipAddress"></param>
		/// <returns></returns>
		static public bool IsValidIPv4(string ipAddress)
		{
			if (string.IsNullOrWhiteSpace(ipAddress))
				return false;

			// We are a little more restrictive than IPAddress
			int dotCount = ipAddress.Count(c => c == '.');
			if (dotCount != 3)
				return false;

			IPAddress ip;
			if (!IPAddress.TryParse(ipAddress, out ip))
				return false;

			return ip.AddressFamily == AddressFamily.InterNetwork;
		}

		/// <summary>
		/// Validates if the string contains a well-formed IP address v6
		/// </summary>
		static public bool IsValidIPv6(string ipAddress)
		{
			if (string.IsNullOrWhiteSpace(ipAddress))
				return false;

			IPAddress ip;
			if (!IPAddress.TryParse(ipAddress, out ip))
				return false;

			return ip.AddressFamily == AddressFamily.InterNetworkV6;
		}

		/// <summary>
		/// Validates if the string contains an IP Address v4 or v6
		/// </summary>
		static public bool IsValidIP(string ipAddress)
		{
			if (string.IsNullOrWhiteSpace(ipAddress))
				return false;

			IPAddress ip;
			if (!IPAddress.TryParse(ipAddress, out ip))
				return false;

			return ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6;
		}

		/// <summary>
		/// Validates a time string
		/// </summary>
		static public bool IsValidTime(string timeString)
		{
			var time = DateTimeExtensions.ParseTime(timeString);
			return time != null;
		}

		/// <summary>
		/// Validates a date string (using Gregorian calendar)
		/// </summary>
		static public bool IsValidDate(string dateString, DatePartsOrder datePartsOrder = DatePartsOrder.MDY)
		{
			if (string.IsNullOrWhiteSpace(dateString))
				return false;

			if (dateString.Length < 5)
				return false;

			var parts = dateString.Split(new char[] {'.', '/'});
			if (parts.Length != 3)
				return false;

			Func<string, int> parsePart = s =>
			{
				s = s.Trim();
				if (s.Length == 0)
					return -1;

				int ret = 0;
				if (!int.TryParse(s, out ret))
					return -1;
				return ret;
			};

			int part1 = parsePart(parts[0]);
			if (part1 == -1)
				return false;

			int part2 = parsePart(parts[1]);
			if (part2 == -1)
				return false;

			int part3 = parsePart(parts[2]);
			if (part3 == -1)
				return false;

			int m, d, y;
			switch (datePartsOrder)
			{
				case DatePartsOrder.MDY:
					m = part1;
					d = part2;
					y = part3;
					break;
				case DatePartsOrder.YMD:
					y = part1;
					m = part2;
					d = part3;
					break;
				case DatePartsOrder.DMY:
					d = part1;
					m = part2;
					y = part3;
					break;
				default:
					throw new NotImplementedException("Unsupported DatePartsOrder.");
			}

			// Up for debate, but in practical terms, a year so much in the future is probably not
 			// the intent of validating a date in a typical user input scenario.
			if (y < 1 || y > 2200 || m < 1 || m > 12 || d < 1)
				return false;
			if (m == 1 || m == 3 || m == 5 || m == 7 || m == 8 || m == 10 || m == 12)
			{
				if (d > 31)
					return false;
			}
			else if (m == 2)
			{
				if (DateTime.IsLeapYear(y))
				{
					if (d > 29)
						return false;
				}
				else if (d > 28)
					return false;
			}
			else if (d > 30)
			{
				return false;
			}

			return true;
		}


	}
}
