using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.ExtensionsGalore
{
    public static class TimeZoneInfoExtensions
    {
        public static Dictionary<string, string> _OlsonToWindows;
        private static Dictionary<string, string[]> _WindowsToOlson;
        static TimeZoneInfoExtensions()
        {
            //string[] mappingToWindows = new[]
            //{
            //	"Africa/Abidjan", "Greenwich Standard Time",
            //	"Africa/Accra", "Greenwich Standard Time",
            //	"Africa/Addis_Ababa", "E. Africa Standard Time",
            //	"Africa/Algiers", "W. Central Africa Standard Time",
            //	"Africa/Asmera", "E. Africa Standard Time",
            //	"Africa/Bamako", "Greenwich Standard Time",
            //	"Africa/Bangui", "W. Central Africa Standard Time",
            //	"Africa/Banjul", "Greenwich Standard Time",
            //	"Africa/Bissau", "Greenwich Standard Time",
            //	"Africa/Blantyre", "South Africa Standard Time",
            //	"Africa/Brazzaville", "W. Central Africa Standard Time",
            //	"Africa/Bujumbura", "South Africa Standard Time",
            //	"Africa/Cairo", "Egypt Standard Time",
            //	"Africa/Casablanca", "Morocco Standard Time",
            //	"Africa/Ceuta", "Romance Standard Time",
            //	"Africa/Conakry", "Greenwich Standard Time",
            //	"Africa/Dakar", "Greenwich Standard Time",
            //	"Africa/Dar_es_Salaam", "E. Africa Standard Time",
            //	"Africa/Djibouti", "E. Africa Standard Time",
            //	"Africa/Douala", "W. Central Africa Standard Time",
            //	"Africa/El_Aaiun", "Greenwich Standard Time",
            //	"Africa/Freetown", "Greenwich Standard Time",
            //	"Africa/Gaborone", "South Africa Standard Time",
            //	"Africa/Harare", "South Africa Standard Time",
            //	"Africa/Johannesburg", "South Africa Standard Time",
            //	"Africa/Juba", "E. Africa Standard Time",
            //	"Africa/Kampala", "E. Africa Standard Time",
            //	"Africa/Khartoum", "E. Africa Standard Time",
            //	"Africa/Kigali", "South Africa Standard Time",
            //	"Africa/Kinshasa", "W. Central Africa Standard Time",
            //	"Africa/Lagos", "W. Central Africa Standard Time",
            //	"Africa/Libreville", "W. Central Africa Standard Time",
            //	"Africa/Lome", "Greenwich Standard Time",
            //	"Africa/Luanda", "W. Central Africa Standard Time",
            //	"Africa/Lubumbashi", "South Africa Standard Time",
            //	"Africa/Lusaka", "South Africa Standard Time",
            //	"Africa/Malabo", "W. Central Africa Standard Time",
            //	"Africa/Maputo", "South Africa Standard Time",
            //	"Africa/Maseru", "South Africa Standard Time",
            //	"Africa/Mbabane", "South Africa Standard Time",
            //	"Africa/Mogadishu", "E. Africa Standard Time",
            //	"Africa/Monrovia", "Greenwich Standard Time",
            //	"Africa/Nairobi", "E. Africa Standard Time",
            //	"Africa/Ndjamena", "W. Central Africa Standard Time",
            //	"Africa/Niamey", "W. Central Africa Standard Time",
            //	"Africa/Nouakchott", "Greenwich Standard Time",
            //	"Africa/Ouagadougou", "Greenwich Standard Time",
            //	"Africa/Porto-Novo", "W. Central Africa Standard Time",
            //	"Africa/Sao_Tome", "Greenwich Standard Time",
            //	"Africa/Tripoli", "W. Europe Standard Time",
            //	"Africa/Tunis", "W. Central Africa Standard Time",
            //	"Africa/Windhoek", "Namibia Standard Time",
            //	"America/Anchorage", "Alaskan Standard Time",
            //	"America/Anguilla", "SA Western Standard Time",
            //	"America/Antigua", "SA Western Standard Time",
            //	"America/Araguaina", "SA Eastern Standard Time",
            //	"America/Argentina/Buenos_Aires", "Argentina Standard Time",
            //	"America/Argentina/Cordoba", "Argentina Standard Time",
            //	"America/Argentina/Rio_Gallegos", "Argentina Standard Time",
            //	"America/Argentina/Salta", "Argentina Standard Time",
            //	"America/Argentina/San_Juan", "Argentina Standard Time",
            //	"America/Argentina/San_Luis", "Argentina Standard Time",
            //	"America/Argentina/Tucuman", "Argentina Standard Time",
            //	"America/Argentina/Ushuaia", "Argentina Standard Time",
            //	"America/Aruba", "SA Western Standard Time",
            //	"America/Asuncion", "Paraguay Standard Time",
            //	"America/Atikokan", "Eastern Standard Time",
            //	"America/Bahia", "Bahia Standard Time",
            //	"America/Bahia_Banderas", "Central Standard Time (Mexico)",
            //	"America/Barbados", "SA Western Standard Time",
            //	"America/Belem", "SA Eastern Standard Time",
            //	"America/Belize", "Central America Standard Time",
            //	"America/Blanc-Sablon", "SA Western Standard Time",
            //	"America/Boa_Vista", "SA Western Standard Time",
            //	"America/Bogota", "SA Pacific Standard Time",
            //	"America/Boise", "Mountain Standard Time",
            //	"America/Buenos_Aires", "Argentina Standard Time",
            //	"America/Cambridge_Bay", "Mountain Standard Time",
            //	"America/Campo_Grande", "Central Brazilian Standard Time",
            //	"America/Cancun", "Central Standard Time (Mexico)",
            //	"America/Caracas", "Venezuela Standard Time",
            //	"America/Catamarca", "Argentina Standard Time",
            //	"America/Cayenne", "SA Eastern Standard Time",
            //	"America/Cayman", "SA Pacific Standard Time",
            //	"America/Chicago", "Central Standard Time",
            //	"America/Chihuahua", "Mountain Standard Time (Mexico)",
            //	"America/Coral_Harbour", "SA Pacific Standard Time",
            //	"America/Cordoba", "Argentina Standard Time",
            //	"America/Costa_Rica", "Central America Standard Time",
            //	"America/Creston", "US Mountain Standard Time",
            //	"America/Cuiaba", "Central Brazilian Standard Time",
            //	"America/Curacao", "SA Western Standard Time",
            //	"America/Danmarkshavn", "UTC",
            //	"America/Dawson", "Pacific Standard Time",
            //	"America/Dawson_Creek", "US Mountain Standard Time",
            //	"America/Denver", "Mountain Standard Time",
            //	"America/Detroit", "Eastern Standard Time",
            //	"America/Dominica", "SA Western Standard Time",
            //	"America/Edmonton", "Mountain Standard Time",
            //	"America/Eirunepe", "SA Western Standard Time",
            //	"America/El_Salvador", "Central America Standard Time",
            //	"America/Fortaleza", "SA Eastern Standard Time",
            //	"America/Glace_Bay", "Atlantic Standard Time",
            //	"America/Godthab", "Greenland Standard Time",
            //	"America/Goose_Bay", "Atlantic Standard Time",
            //	"America/Grand_Turk", "Eastern Standard Time",
            //	"America/Grenada", "SA Western Standard Time",
            //	"America/Guadeloupe", "SA Western Standard Time",
            //	"America/Guatemala", "Central America Standard Time",
            //	"America/Guayaquil", "SA Pacific Standard Time",
            //	"America/Guyana", "SA Western Standard Time",
            //	"America/Halifax", "Atlantic Standard Time",
            //	"America/Hermosillo", "US Mountain Standard Time",
            //	"America/Indiana/Indianapolis", "US Eastern Standard Time",
            //	"America/Indiana/Knox", "Central Standard Time",
            //	"America/Indiana/Marengo", "US Eastern Standard Time",
            //	"America/Indiana/Petersburg", "Eastern Standard Time",
            //	"America/Indiana/Tell_City", "Central Standard Time",
            //	"America/Indiana/Vevay", "US Eastern Standard Time",
            //	"America/Indiana/Vincennes", "Eastern Standard Time",
            //	"America/Indiana/Winamac", "Eastern Standard Time",
            //	"America/Indianapolis", "US Eastern Standard Time",
            //	"America/Inuvik", "Mountain Standard Time",
            //	"America/Iqaluit", "Eastern Standard Time",
            //	"America/Jamaica", "SA Pacific Standard Time",
            //	"America/Jujuy", "Argentina Standard Time",
            //	"America/Juneau", "Alaskan Standard Time",
            //	"America/Kentucky/Louisville", "Eastern Standard Time",
            //	"America/Kentucky/Monticello", "Eastern Standard Time",
            //	"America/Kralendijk", "SA Western Standard Time",
            //	"America/La_Paz", "SA Western Standard Time",
            //	"America/Lima", "SA Pacific Standard Time",
            //	"America/Los_Angeles", "Pacific Standard Time",
            //	"America/Louisville", "Eastern Standard Time",
            //	"America/Lower_Princes", "SA Western Standard Time",
            //	"America/Maceio", "SA Eastern Standard Time",
            //	"America/Managua", "Central America Standard Time",
            //	"America/Manaus", "SA Western Standard Time",
            //	"America/Marigot", "SA Western Standard Time",
            //	"America/Martinique", "SA Western Standard Time",
            //	"America/Matamoros", "Central Standard Time",
            //	"America/Mazatlan", "Mountain Standard Time (Mexico)",
            //	"America/Mendoza", "Argentina Standard Time",
            //	"America/Menominee", "Central Standard Time",
            //	"America/Merida", "Central Standard Time (Mexico)",
            //	"America/Mexico_City", "Central Standard Time (Mexico)",
            //	"America/Moncton", "Atlantic Standard Time",
            //	"America/Monterrey", "Central Standard Time (Mexico)",
            //	"America/Montevideo", "Montevideo Standard Time",
            //	"America/Montreal", "Eastern Standard Time",
            //	"America/Montserrat", "SA Western Standard Time",
            //	"America/Nassau", "Eastern Standard Time",
            //	"America/New_York", "Eastern Standard Time",
            //	"America/Nipigon", "Eastern Standard Time",
            //	"America/Nome", "Alaskan Standard Time",
            //	"America/Noronha", "UTC-02",
            //	"America/North_Dakota/Beulah", "Central Standard Time",
            //	"America/North_Dakota/Center", "Central Standard Time",
            //	"America/North_Dakota/New_Salem", "Central Standard Time",
            //	"America/Ojinaga", "Mountain Standard Time",
            //	"America/Panama", "SA Pacific Standard Time",
            //	"America/Pangnirtung", "Eastern Standard Time",
            //	"America/Paramaribo", "SA Eastern Standard Time",
            //	"America/Phoenix", "US Mountain Standard Time",
            //	"America/Port_of_Spain", "SA Western Standard Time",
            //	"America/Port-au-Prince", "SA Pacific Standard Time",
            //	"America/Porto_Velho", "SA Western Standard Time",
            //	"America/Puerto_Rico", "SA Western Standard Time",
            //	"America/Rainy_River", "Central Standard Time",
            //	"America/Rankin_Inlet", "Central Standard Time",
            //	"America/Recife", "SA Eastern Standard Time",
            //	"America/Regina", "Canada Central Standard Time",
            //	"America/Resolute", "Central Standard Time",
            //	"America/Rio_Branco", "SA Western Standard Time",
            //	"America/Santa_Isabel", "Pacific Standard Time (Mexico)",
            //	"America/Santarem", "SA Eastern Standard Time",
            //	"America/Santiago", "Pacific SA Standard Time",
            //	"America/Santo_Domingo", "SA Western Standard Time",
            //	"America/Sao_Paulo", "E. South America Standard Time",
            //	"America/Scoresbysund", "Azores Standard Time",
            //	"America/Shiprock", "Mountain Standard Time",
            //	"America/Sitka", "Alaskan Standard Time",
            //	"America/St_Barthelemy", "SA Western Standard Time",
            //	"America/St_Johns", "Newfoundland Standard Time",
            //	"America/St_Kitts", "SA Western Standard Time",
            //	"America/St_Lucia", "SA Western Standard Time",
            //	"America/St_Thomas", "SA Western Standard Time",
            //	"America/St_Vincent", "SA Western Standard Time",
            //	"America/Swift_Current", "Canada Central Standard Time",
            //	"America/Tegucigalpa", "Central America Standard Time",
            //	"America/Thule", "Atlantic Standard Time",
            //	"America/Thunder_Bay", "Eastern Standard Time",
            //	"America/Tijuana", "Pacific Standard Time",
            //	"America/Toronto", "Eastern Standard Time",
            //	"America/Tortola", "SA Western Standard Time",
            //	"America/Vancouver", "Pacific Standard Time",
            //	"America/Whitehorse", "Pacific Standard Time",
            //	"America/Winnipeg", "Central Standard Time",
            //	"America/Yakutat", "Alaskan Standard Time",
            //	"America/Yellowknife", "Mountain Standard Time",
            //	"Antarctica/Casey", "W. Australia Standard Time",
            //	"Antarctica/Davis", "SE Asia Standard Time",
            //	"Antarctica/DumontDUrville", "West Pacific Standard Time",
            //	"Antarctica/Macquarie", "Central Pacific Standard Time",
            //	"Antarctica/Mawson", "West Asia Standard Time",
            //	"Antarctica/McMurdo", "New Zealand Standard Time",
            //	"Antarctica/Palmer", "Pacific SA Standard Time",
            //	"Antarctica/Rothera", "SA Eastern Standard Time",
            //	"Antarctica/South_Pole", "New Zealand Standard Time",
            //	"Antarctica/Syowa", "E. Africa Standard Time",
            //	"Antarctica/Vostok", "Central Asia Standard Time",
            //	"Arctic/Longyearbyen", "W. Europe Standard Time",
            //	"Asia/Aden", "Arab Standard Time",
            //	"Asia/Almaty", "Central Asia Standard Time",
            //	"Asia/Amman", "Jordan Standard Time",
            //	"Asia/Anadyr", "Magadan Standard Time",
            //	"Asia/Aqtau", "West Asia Standard Time",
            //	"Asia/Aqtobe", "West Asia Standard Time",
            //	"Asia/Ashgabat", "West Asia Standard Time",
            //	"Asia/Baghdad", "Arabic Standard Time",
            //	"Asia/Bahrain", "Arab Standard Time",
            //	"Asia/Baku", "Azerbaijan Standard Time",
            //	"Asia/Bangkok", "SE Asia Standard Time",
            //	"Asia/Beirut", "Middle East Standard Time",
            //	"Asia/Bishkek", "Central Asia Standard Time",
            //	"Asia/Brunei", "Singapore Standard Time",
            //	"Asia/Calcutta", "India Standard Time",
            //	"Asia/Choibalsan", "Ulaanbaatar Standard Time",
            //	"Asia/Chongqing", "China Standard Time",
            //	"Asia/Colombo", "Sri Lanka Standard Time",
            //	"Asia/Damascus", "Syria Standard Time",
            //	"Asia/Dhaka", "Bangladesh Standard Time",
            //	"Asia/Dili", "Tokyo Standard Time",
            //	"Asia/Dubai", "Arabian Standard Time",
            //	"Asia/Dushanbe", "West Asia Standard Time",
            //	"Asia/Gaza", "Egypt Standard Time",
            //	"Asia/Harbin", "China Standard Time",
            //	"Asia/Hebron", "Egypt Standard Time",
            //	"Asia/Hong_Kong", "China Standard Time",
            //	"Asia/Hovd", "SE Asia Standard Time",
            //	"Asia/Irkutsk", "North Asia East Standard Time",
            //	"Asia/Jakarta", "SE Asia Standard Time",
            //	"Asia/Jayapura", "Tokyo Standard Time",
            //	"Asia/Jerusalem", "Israel Standard Time",
            //	"Asia/Kabul", "Afghanistan Standard Time",
            //	"Asia/Kamchatka", "Magadan Standard Time",
            //	"Asia/Karachi", "Pakistan Standard Time",
            //	"Asia/Kashgar", "China Standard Time",
            //	"Asia/Katmandu", "Nepal Standard Time",
            //	"Asia/Kolkata", "India Standard Time",
            //	"Asia/Krasnoyarsk", "North Asia Standard Time",
            //	"Asia/Kuala_Lumpur", "Singapore Standard Time",
            //	"Asia/Kuching", "Singapore Standard Time",
            //	"Asia/Kuwait", "Arab Standard Time",
            //	"Asia/Macau", "China Standard Time",
            //	"Asia/Magadan", "Magadan Standard Time",
            //	"Asia/Makassar", "Singapore Standard Time",
            //	"Asia/Manila", "Singapore Standard Time",
            //	"Asia/Muscat", "Arabian Standard Time",
            //	"Asia/Nicosia", "E. Europe Standard Time",
            //	"Asia/Novokuznetsk", "N. Central Asia Standard Time",
            //	"Asia/Novosibirsk", "N. Central Asia Standard Time",
            //	"Asia/Omsk", "N. Central Asia Standard Time",
            //	"Asia/Oral", "West Asia Standard Time",
            //	"Asia/Phnom_Penh", "SE Asia Standard Time",
            //	"Asia/Pontianak", "SE Asia Standard Time",
            //	"Asia/Pyongyang", "Korea Standard Time",
            //	"Asia/Qatar", "Arab Standard Time",
            //	"Asia/Qyzylorda", "Central Asia Standard Time",
            //	"Asia/Rangoon", "Myanmar Standard Time",
            //	"Asia/Riyadh", "Arab Standard Time",
            //	"Asia/Saigon", "SE Asia Standard Time",
            //	"Asia/Sakhalin", "Vladivostok Standard Time",
            //	"Asia/Samarkand", "West Asia Standard Time",
            //	"Asia/Seoul", "Korea Standard Time",
            //	"Asia/Shanghai", "China Standard Time",
            //	"Asia/Singapore", "Singapore Standard Time",
            //	"Asia/Taipei", "Taipei Standard Time",
            //	"Asia/Tashkent", "West Asia Standard Time",
            //	"Asia/Tbilisi", "Georgian Standard Time",
            //	"Asia/Tehran", "Iran Standard Time",
            //	"Asia/Thimphu", "Bangladesh Standard Time",
            //	"Asia/Tokyo", "Tokyo Standard Time",
            //	"Asia/Ulaanbaatar", "Ulaanbaatar Standard Time",
            //	"Asia/Urumqi", "China Standard Time",
            //	"Asia/Vientiane", "SE Asia Standard Time",
            //	"Asia/Vladivostok", "Vladivostok Standard Time",
            //	"Asia/Yakutsk", "Yakutsk Standard Time",
            //	"Asia/Yekaterinburg", "Ekaterinburg Standard Time",
            //	"Asia/Yerevan", "Caucasus Standard Time",
            //	"Atlantic/Azores", "Azores Standard Time",
            //	"Atlantic/Bermuda", "Atlantic Standard Time",
            //	"Atlantic/Canary", "GMT Standard Time",
            //	"Atlantic/Cape_Verde", "Cape Verde Standard Time",
            //	"Atlantic/Faeroe", "GMT Standard Time",
            //	"Atlantic/Madeira", "GMT Standard Time",
            //	"Atlantic/Reykjavik", "Greenwich Standard Time",
            //	"Atlantic/South_Georgia", "UTC-02",
            //	"Atlantic/St_Helena", "Greenwich Standard Time",
            //	"Atlantic/Stanley", "SA Eastern Standard Time",
            //	"Australia/Adelaide", "Cen. Australia Standard Time",
            //	"Australia/Brisbane", "E. Australia Standard Time",
            //	"Australia/Broken_Hill", "Cen. Australia Standard Time",
            //	"Australia/Currie", "Tasmania Standard Time",
            //	"Australia/Darwin", "AUS Central Standard Time",
            //	"Australia/Hobart", "Tasmania Standard Time",
            //	"Australia/Lindeman", "E. Australia Standard Time",
            //	"Australia/Melbourne", "AUS Eastern Standard Time",
            //	"Australia/Perth", "W. Australia Standard Time",
            //	"Australia/Sydney", "AUS Eastern Standard Time",
            //	"Canada/Pacific", "Pacific Standard Time", // Linked to America/Vancouver
            //	"CST6CDT", "Central Standard Time",
            //	"EST5EDT", "Eastern Standard Time",
            //	"Etc/GMT", "UTC",
            //	"Etc/GMT+1", "Cape Verde Standard Time",
            //	"Etc/GMT+10", "Hawaiian Standard Time",
            //	"Etc/GMT+11", "UTC-11",
            //	"Etc/GMT+12", "Dateline Standard Time",
            //	"Etc/GMT+2", "UTC-02",
            //	"Etc/GMT+3", "SA Eastern Standard Time",
            //	"Etc/GMT+4", "SA Western Standard Time",
            //	"Etc/GMT+5", "SA Pacific Standard Time",
            //	"Etc/GMT+6", "Central America Standard Time",
            //	"Etc/GMT+7", "US Mountain Standard Time",
            //	"Etc/GMT-1", "W. Central Africa Standard Time",
            //	"Etc/GMT-10", "West Pacific Standard Time",
            //	"Etc/GMT-11", "Central Pacific Standard Time",
            //	"Etc/GMT-12", "UTC+12",
            //	"Etc/GMT-13", "Tonga Standard Time",
            //	"Etc/GMT-2", "South Africa Standard Time",
            //	"Etc/GMT-3", "E. Africa Standard Time",
            //	"Etc/GMT-4", "Arabian Standard Time",
            //	"Etc/GMT-5", "West Asia Standard Time",
            //	"Etc/GMT-6", "Central Asia Standard Time",
            //	"Etc/GMT-7", "SE Asia Standard Time",
            //	"Etc/GMT-8", "Singapore Standard Time",
            //	"Etc/GMT-9", "Tokyo Standard Time",
            //	"Europe/Amsterdam", "W. Europe Standard Time",
            //	"Europe/Andorra", "W. Europe Standard Time",
            //	"Europe/Athens", "GTB Standard Time",
            //	"Europe/Belgrade", "Central Europe Standard Time",
            //	"Europe/Berlin", "W. Europe Standard Time",
            //	"Europe/Bratislava", "Central Europe Standard Time",
            //	"Europe/Brussels", "Romance Standard Time",
            //	"Europe/Bucharest", "GTB Standard Time",
            //	"Europe/Budapest", "Central Europe Standard Time",
            //	"Europe/Chisinau", "GTB Standard Time",
            //	"Europe/Copenhagen", "Romance Standard Time",
            //	"Europe/Dublin", "GMT Standard Time",
            //	"Europe/Gibraltar", "W. Europe Standard Time",
            //	"Europe/Guernsey", "GMT Standard Time",
            //	"Europe/Helsinki", "FLE Standard Time",
            //	"Europe/Isle_of_Man", "GMT Standard Time",
            //	"Europe/Istanbul", "Turkey Standard Time",
            //	"Europe/Jersey", "GMT Standard Time",
            //	"Europe/Kaliningrad", "Kaliningrad Standard Time",
            //	"Europe/Kiev", "FLE Standard Time",
            //	"Europe/Lisbon", "GMT Standard Time",
            //	"Europe/Ljubljana", "Central Europe Standard Time",
            //	"Europe/London", "GMT Standard Time",
            //	"Europe/Luxembourg", "W. Europe Standard Time",
            //	"Europe/Madrid", "Romance Standard Time",
            //	"Europe/Malta", "W. Europe Standard Time",
            //	"Europe/Mariehamn", "FLE Standard Time",
            //	"Europe/Minsk", "Kaliningrad Standard Time",
            //	"Europe/Monaco", "W. Europe Standard Time",
            //	"Europe/Moscow", "Russian Standard Time",
            //	"Europe/Oslo", "W. Europe Standard Time",
            //	"Europe/Paris", "Romance Standard Time",
            //	"Europe/Podgorica", "Central Europe Standard Time",
            //	"Europe/Prague", "Central Europe Standard Time",
            //	"Europe/Riga", "FLE Standard Time",
            //	"Europe/Rome", "W. Europe Standard Time",
            //	"Europe/Samara", "Russian Standard Time",
            //	"Europe/San_Marino", "W. Europe Standard Time",
            //	"Europe/Sarajevo", "Central European Standard Time",
            //	"Europe/Simferopol", "FLE Standard Time",
            //	"Europe/Skopje", "Central European Standard Time",
            //	"Europe/Sofia", "FLE Standard Time",
            //	"Europe/Stockholm", "W. Europe Standard Time",
            //	"Europe/Tallinn", "FLE Standard Time",
            //	"Europe/Tirane", "Central Europe Standard Time",
            //	"Europe/Uzhgorod", "FLE Standard Time",
            //	"Europe/Vaduz", "W. Europe Standard Time",
            //	"Europe/Vatican", "W. Europe Standard Time",
            //	"Europe/Vienna", "W. Europe Standard Time",
            //	"Europe/Vilnius", "FLE Standard Time",
            //	"Europe/Volgograd", "Russian Standard Time",
            //	"Europe/Warsaw", "Central European Standard Time",
            //	"Europe/Zagreb", "Central European Standard Time",
            //	"Europe/Zaporozhye", "FLE Standard Time",
            //	"Europe/Zurich", "W. Europe Standard Time",
            //	"GMT", "Greenwich Standard Time",
            //	"Indian/Antananarivo", "E. Africa Standard Time",
            //	"Indian/Chagos", "Central Asia Standard Time",
            //	"Indian/Christmas", "SE Asia Standard Time",
            //	"Indian/Cocos", "Myanmar Standard Time",
            //	"Indian/Comoro", "E. Africa Standard Time",
            //	"Indian/Kerguelen", "West Asia Standard Time",
            //	"Indian/Mahe", "Mauritius Standard Time",
            //	"Indian/Maldives", "West Asia Standard Time",
            //	"Indian/Mauritius", "Mauritius Standard Time",
            //	"Indian/Mayotte", "E. Africa Standard Time",
            //	"Indian/Reunion", "Mauritius Standard Time",
            //	"MST7MDT", "Mountain Standard Time",
            //	"Pacific/Apia", "Samoa Standard Time",
            //	"Pacific/Auckland", "New Zealand Standard Time",
            //	"Pacific/Efate", "Central Pacific Standard Time",
            //	"Pacific/Enderbury", "Tonga Standard Time",
            //	"Pacific/Fakaofo", "Tonga Standard Time",
            //	"Pacific/Fiji", "Fiji Standard Time",
            //	"Pacific/Funafuti", "UTC+12",
            //	"Pacific/Galapagos", "Central America Standard Time",
            //	"Pacific/Guadalcanal", "Central Pacific Standard Time",
            //	"Pacific/Guam", "West Pacific Standard Time",
            //	"Pacific/Honolulu", "Hawaiian Standard Time",
            //	"Pacific/Johnston", "Hawaiian Standard Time",
            //	"Pacific/Kosrae", "Central Pacific Standard Time",
            //	"Pacific/Kwajalein", "UTC+12",
            //	"Pacific/Majuro", "UTC+12",
            //	"Pacific/Midway", "UTC-11",
            //	"Pacific/Nauru", "UTC+12",
            //	"Pacific/Niue", "UTC-11",
            //	"Pacific/Noumea", "Central Pacific Standard Time",
            //	"Pacific/Pago_Pago", "UTC-11",
            //	"Pacific/Palau", "Tokyo Standard Time",
            //	"Pacific/Pitcairn", "Pacific Standard Time",
            //	"Pacific/Ponape", "Central Pacific Standard Time",
            //	"Pacific/Port_Moresby", "West Pacific Standard Time",
            //	"Pacific/Rarotonga", "Hawaiian Standard Time",
            //	"Pacific/Saipan", "West Pacific Standard Time",
            //	"Pacific/Tahiti", "Hawaiian Standard Time",
            //	"Pacific/Tarawa", "UTC+12",
            //	"Pacific/Tongatapu", "Tonga Standard Time",
            //	"Pacific/Truk", "West Pacific Standard Time",
            //	"Pacific/Wake", "UTC+12",
            //	"Pacific/Wallis", "UTC+12",
            //	"PST8PDT", "Pacific Standard Time",
            //	"US/Alaska", "Alaskan Standard Time",
            //	"US/Arizona", "US Mountain Standard Time",
            //	"US/Central", "Central Standard Time",
            //	"US/Eastern", "Eastern Standard Time",
            //	"US/Hawaii", "Hawaiian Standard Time",
            //	"US/Mountain", "US Mountain Standard Time",
            //	"US/Pacific", "Pacific Standard Time",
            //};


            //for (int i = 0; i < mappingToWindows.Length; i += 2)
            //{
            //	string olson = mappingToWindows[i];
            //	string windows = mappingToWindows[i + 1];
            //	_OlsonToWindows[olson] = windows;
            //}

            // The first mapping is the most popular one
            _WindowsToOlson = new Dictionary<string, string[]>(StringComparer.InvariantCultureIgnoreCase);

            _WindowsToOlson["Afghanistan Standard Time"] = new[] { "Asia/Kabul", };
            _WindowsToOlson["Alaskan Standard Time"] = new[]
            {
                "US/Alaska",
                "America/Anchorage",
                "America/Juneau",
                "America/Nome",
                "America/Sitka",
                "America/Yakutat",
            };
            _WindowsToOlson["Arab Standard Time"] = new[]
            {
                "Asia/Riyadh",
                "Asia/Aden",
                "Asia/Bahrain",
                "Asia/Kuwait",
                "Asia/Qatar",
            };
            _WindowsToOlson["Arabian Standard Time"] = new[]
            {
                "Asia/Dubai",
                "Asia/Muscat",
                "Etc/GMT-4",
            };
            _WindowsToOlson["Arabic Standard Time"] = new[]
            {
                "Asia/Baghdad",
            };
            _WindowsToOlson["Argentina Standard Time"] = new[]
            {
                "America/Buenos_Aires",
                "America/Argentina/Buenos_Aires",
                "America/Argentina/Cordoba",
                "America/Argentina/Rio_Gallegos",
                "America/Argentina/Salta",
                "America/Argentina/San_Juan",
                "America/Argentina/San_Luis",
                "America/Argentina/Tucuman",
                "America/Argentina/Ushuaia",
                "America/Catamarca",
                "America/Cordoba",
                "America/Jujuy",
                "America/Mendoza",
            };
            _WindowsToOlson["Atlantic Standard Time"] = new[]
            {
                "America/Halifax",
                "America/Glace_Bay",
                "America/Goose_Bay",
                "America/Moncton",
                "America/Thule",
                "Atlantic/Bermuda",
            };
            _WindowsToOlson["AUS Central Standard Time"] = new[]
            {
                "Australia/Darwin",
            };
            _WindowsToOlson["AUS Eastern Standard Time"] = new[]
            {
                "Australia/Sydney",
                "Australia/Melbourne",
            };
            _WindowsToOlson["Azerbaijan Standard Time"] = new[]
            {
                "Asia/Baku",
            };
            _WindowsToOlson["Azores Standard Time"] = new[]
            {
                "Atlantic/Azores",
                "America/Scoresbysund",
            };
            _WindowsToOlson["Bahia Standard Time"] = new[]
            {
                "America/Bahia",
            };
            _WindowsToOlson["Bangladesh Standard Time"] = new[]
            {
                "Asia/Dhaka",
                "Asia/Thimphu",
            };
            _WindowsToOlson["Canada Central Standard Time"] = new[]
            {
                "America/Regina",
                "America/Swift_Current",
            };
            _WindowsToOlson["Cape Verde Standard Time"] = new[]
            {
                "Atlantic/Cape_Verde",
                "Etc/GMT+1",
            };
            _WindowsToOlson["Caucasus Standard Time"] = new[]
            {
                "Asia/Yerevan",
            };
            _WindowsToOlson["Cen. Australia Standard Time"] = new[]
            {
                "Australia/Adelaide",
                "Australia/Broken_Hill",
            };
            _WindowsToOlson["Central America Standard Time"] = new[]
            {
                "America/Guatemala",
                "America/Belize",
                "America/Costa_Rica",
                "America/El_Salvador",
                "America/Managua",
                "America/Tegucigalpa",
                "Etc/GMT+6",
                "Pacific/Galapagos",
            };
            _WindowsToOlson["Central Asia Standard Time"] = new[]
            {
                "Asia/Almaty",
                "Antarctica/Vostok",
                "Asia/Bishkek",
                "Asia/Qyzylorda",
                "Indian/Chagos",
                "Etc/GMT-6",
            };
            _WindowsToOlson["Central Brazilian Standard Time"] = new[]
            {
                "America/Campo_Grande",
                "America/Cuiaba",
            };
            _WindowsToOlson["Central Europe Standard Time"] = new[]
            {
                "Europe/Budapest",
                "Europe/Belgrade",
                "Europe/Bratislava",
                "Europe/Ljubljana",
                "Europe/Podgorica",
                "Europe/Prague",
                "Europe/Tirane",
            };
            _WindowsToOlson["Central European Standard Time"] = new[]
            {
                "Europe/Warsaw",
                "Europe/Sarajevo",
                "Europe/Skopje",
                "Europe/Zagreb",
            };
            _WindowsToOlson["Central Pacific Standard Time"] = new[]
            {
                "Pacific/Guadalcanal",
                "Antarctica/Macquarie",
                "Pacific/Efate",
                "Pacific/Kosrae",
                "Pacific/Noumea",
                "Pacific/Ponape",
                "Etc/GMT-11",
            };
            _WindowsToOlson["Central Standard Time"] = new[]
            {
                "US/Central",
                "America/Chicago",
                "America/Indiana/Knox",
                "America/Indiana/Tell_City",
                "America/Matamoros",
                "America/Menominee",
                "America/North_Dakota/Beulah",
                "America/North_Dakota/Center",
                "America/North_Dakota/New_Salem",
                "America/Rainy_River",
                "America/Rankin_Inlet",
                "America/Resolute",
                "America/Winnipeg",
                "CST6CDT",
            };
            _WindowsToOlson["Central Standard Time (Mexico)"] = new[]
            {
                "America/Mexico_City",
                "America/Bahia_Banderas",
                "America/Merida",
                "America/Monterrey",
            };
            _WindowsToOlson["China Standard Time"] = new[]
            {
                "Asia/Shanghai",
                "Asia/Chongqing",
                "Asia/Harbin",
                "Asia/Hong_Kong",
                "Asia/Kashgar",
                "Asia/Macau",
                "Asia/Urumqi",
            };
            _WindowsToOlson["Dateline Standard Time"] = new[]
            {
                "Etc/GMT+12",
            };
            _WindowsToOlson["E. Africa Standard Time"] = new[]
            {
                "Africa/Khartoum",
                "Africa/Addis_Ababa",
                "Africa/Asmera",
                "Africa/Dar_es_Salaam",
                "Africa/Djibouti",
                "Africa/Juba",
                "Africa/Kampala",
                "Africa/Mogadishu",
                "Africa/Nairobi",
                "Antarctica/Syowa",
                "Indian/Antananarivo",
                "Indian/Comoro",
                "Indian/Mayotte",
                "Etc/GMT-3",
            };
            _WindowsToOlson["E. Australia Standard Time"] = new[]
            {
                "Australia/Brisbane",
                "Australia/Lindeman",
            };
            _WindowsToOlson["E. Europe Standard Time"] = new[]
            {
                "Asia/Nicosia",
            };
            _WindowsToOlson["E. South America Standard Time"] = new[]
            {
                "America/Sao_Paulo",
            };
            _WindowsToOlson["Eastern Standard Time"] = new[]
            {
                "US/Eastern",
                "America/Atikokan",
                "America/Detroit",
                "America/Grand_Turk",
                "America/Indiana/Petersburg",
                "America/Indiana/Vincennes",
                "America/Indiana/Winamac",
                "America/Iqaluit",
                "America/Kentucky/Louisville",
                "America/Kentucky/Monticello",
                "America/Louisville",
                "America/Montreal",
                "America/Nassau",
                "America/New_York",
                "America/Nipigon",
                "America/Pangnirtung",
                "America/Thunder_Bay",
                "America/Toronto",
                "EST5EDT",
            };
            _WindowsToOlson["Eastern Standard Time (Mexico)"] = new[]
            {
                "America/Cancun",
            };
            _WindowsToOlson["Egypt Standard Time"] = new[]
            {
                "Africa/Cairo",
                "Asia/Gaza",
                "Asia/Hebron",
            };
            _WindowsToOlson["Ekaterinburg Standard Time"] = new[]
            {
                "Asia/Yekaterinburg",
            };
            _WindowsToOlson["Fiji Standard Time"] = new[]
            {
                "Pacific/Fiji",
            };
            _WindowsToOlson["FLE Standard Time"] = new[]
            {
                "Europe/Kiev",
                "Europe/Helsinki",
                "Europe/Mariehamn",
                "Europe/Riga",
                "Europe/Simferopol",
                "Europe/Sofia",
                "Europe/Tallinn",
                "Europe/Uzhgorod",
                "Europe/Vilnius",
                "Europe/Zaporozhye",
            };
            _WindowsToOlson["Georgian Standard Time"] = new[]
            {
                "Asia/Tbilisi",
            };
            _WindowsToOlson["GMT Standard Time"] = new[]
            {
                "Europe/London",
                "Atlantic/Canary",
                "Atlantic/Faeroe",
                "Atlantic/Madeira",
                "Europe/Dublin",
                "Europe/Guernsey",
                "Europe/Isle_of_Man",
                "Europe/Jersey",
                "Europe/Lisbon",
            };
            _WindowsToOlson["Greenland Standard Time"] = new[]
            {
                "America/Godthab",
            };
            _WindowsToOlson["Greenwich Standard Time"] = new[]
            {
                "GMT",
                "Africa/Abidjan",
                "Africa/Accra",
                "Africa/Bamako",
                "Africa/Banjul",
                "Africa/Bissau",
                "Africa/Conakry",
                "Africa/Dakar",
                "Africa/El_Aaiun",
                "Africa/Freetown",
                "Africa/Lome",
                "Africa/Monrovia",
                "Africa/Nouakchott",
                "Africa/Ouagadougou",
                "Africa/Sao_Tome",
                "Atlantic/Reykjavik",
                "Atlantic/St_Helena",
            };
            _WindowsToOlson["GTB Standard Time"] = new[]
            {
                "Europe/Bucharest",
                "Europe/Athens",
                "Europe/Chisinau",
            };
            _WindowsToOlson["Hawaiian Standard Time"] = new[]
            {
                "US/Hawaii",
                "Pacific/Honolulu",
                "Pacific/Johnston",
                "Pacific/Rarotonga",
                "Pacific/Tahiti",
                "Etc/GMT+10",
            };
            _WindowsToOlson["India Standard Time"] = new[]
            {
                "Asia/Calcutta",
                "Asia/Kolkata",
            };
            _WindowsToOlson["Iran Standard Time"] = new[]
            {
                "Asia/Tehran",
            };
            _WindowsToOlson["Israel Standard Time"] = new[]
            {
                "Asia/Jerusalem",
            };
            _WindowsToOlson["Jordan Standard Time"] = new[]
            {
                "Asia/Amman",
            };
            _WindowsToOlson["Kaliningrad Standard Time"] = new[]
            {
                "Europe/Minsk",
                "Europe/Kaliningrad",
            };
            _WindowsToOlson["Korea Standard Time"] = new[]
            {
                "Asia/Seoul",
                "Asia/Pyongyang",
            };
            _WindowsToOlson["Line Islands Standard Time"] = new[]
            {
                "Pacific/Kiritimati",
            };
            _WindowsToOlson["Magadan Standard Time"] = new[]
            {
                "Asia/Kamchatka",
                "Asia/Anadyr",
                "Asia/Magadan",
            };
            _WindowsToOlson["Mauritius Standard Time"] = new[]
            {
                "Indian/Mauritius",
                "Indian/Mahe",
                "Indian/Reunion",
            };
            _WindowsToOlson["Middle East Standard Time"] = new[]
            {
                "Asia/Beirut",
            };
            _WindowsToOlson["Montevideo Standard Time"] = new[]
            {
                "America/Montevideo",
            };
            _WindowsToOlson["Morocco Standard Time"] = new[]
            {
                "Africa/Casablanca",
            };
            _WindowsToOlson["Mountain Standard Time"] = new[]
            {
                "America/Denver",
                "America/Boise",
                "America/Cambridge_Bay",
                "America/Edmonton",
                "America/Inuvik",
                "America/Ojinaga",
                "America/Shiprock",
                "America/Yellowknife",
                "MST7MDT",
            };
            _WindowsToOlson["Mountain Standard Time (Mexico)"] = new[]
            {
                "America/Chihuahua",
                "America/Mazatlan",
            };
            _WindowsToOlson["Myanmar Standard Time"] = new[]
            {
                "Asia/Rangoon",
                "Indian/Cocos",
            };
            _WindowsToOlson["N. Central Asia Standard Time"] = new[]
            {
                "Asia/Novosibirsk",
                "Asia/Novokuznetsk",
                "Asia/Omsk",
            };
            _WindowsToOlson["Namibia Standard Time"] = new[]
            {
                "Africa/Windhoek",
            };
            _WindowsToOlson["Nepal Standard Time"] = new[]
            {
                "Asia/Katmandu",
            };
            _WindowsToOlson["New Zealand Standard Time"] = new[]
            {
                "Pacific/Auckland",
                "Antarctica/McMurdo",
                "Antarctica/South_Pole",
            };
            _WindowsToOlson["Newfoundland Standard Time"] = new[]
            {
                "America/St_Johns",
            };
            _WindowsToOlson["North Asia East Standard Time"] = new[]
            {
                "Asia/Irkutsk",
            };
            _WindowsToOlson["North Asia Standard Time"] = new[]
            {
                "Asia/Krasnoyarsk",
            };
            _WindowsToOlson["Pacific SA Standard Time"] = new[]
            {
                "America/Santiago",
                "Antarctica/Palmer",
            };
            _WindowsToOlson["Pacific Standard Time"] = new[]
            {
                "US/Pacific",
                "America/Dawson",
                "America/Los_Angeles",
                "America/Tijuana",
                "America/Vancouver",
                "America/Whitehorse",
                "Canada/Pacific",
                "Pacific/Pitcairn",
                "PST8PDT",
            };
            _WindowsToOlson["Pacific Standard Time (Mexico)"] = new[]
            {
                "America/Santa_Isabel",
            };
            _WindowsToOlson["Pakistan Standard Time"] = new[]
            {
                "Asia/Karachi",
            };
            _WindowsToOlson["Paraguay Standard Time"] = new[]
            {
                "America/Asuncion",
            };
            _WindowsToOlson["Romance Standard Time"] = new[]
            {
                "Europe/Paris",
                "Africa/Ceuta",
                "Europe/Brussels",
                "Europe/Copenhagen",
                "Europe/Madrid",
            };
            _WindowsToOlson["Russian Standard Time"] = new[]
            {
                "Europe/Moscow",
                "Europe/Samara",
                "Europe/Volgograd",
            };
            _WindowsToOlson["SA Eastern Standard Time"] = new[]
            {
                "America/Recife",
                "America/Araguaina",
                "America/Belem",
                "America/Cayenne",
                "America/Fortaleza",
                "America/Maceio",
                "America/Paramaribo",
                "America/Santarem",
                "Antarctica/Rothera",
                "Atlantic/Stanley",
                "Etc/GMT+3",
            };
            _WindowsToOlson["SA Pacific Standard Time"] = new[]
            {
                "America/Lima",
                "America/Bogota",
                "America/Cayman",
                "America/Coral_Harbour",
                "America/Guayaquil",
                "America/Jamaica",
                "America/Panama",
                "America/Port-au-Prince",
                "Etc/GMT+5",
            };
            _WindowsToOlson["SA Western Standard Time"] = new[]
            {
                "America/Puerto_Rico",
                "America/Anguilla",
                "America/Antigua",
                "America/Aruba",
                "America/Barbados",
                "America/Blanc-Sablon",
                "America/Boa_Vista",
                "America/Curacao",
                "America/Dominica",
                "America/Eirunepe",
                "America/Grenada",
                "America/Guadeloupe",
                "America/Guyana",
                "America/Kralendijk",
                "America/La_Paz",
                "America/Lower_Princes",
                "America/Manaus",
                "America/Marigot",
                "America/Martinique",
                "America/Montserrat",
                "America/Port_of_Spain",
                "America/Porto_Velho",
                "America/Rio_Branco",
                "America/Santo_Domingo",
                "America/St_Barthelemy",
                "America/St_Kitts",
                "America/St_Lucia",
                "America/St_Thomas",
                "America/St_Vincent",
                "America/Tortola",
                "Etc/GMT+4",
            };
            _WindowsToOlson["Samoa Standard Time"] = new[]
            {
                "Pacific/Apia",
            };
            _WindowsToOlson["SE Asia Standard Time"] = new[]
            {
                "Asia/Saigon",
                "Antarctica/Davis",
                "Asia/Bangkok",
                "Asia/Hovd",
                "Asia/Jakarta",
                "Asia/Phnom_Penh",
                "Asia/Pontianak",
                "Asia/Vientiane",
                "Indian/Christmas",
                "Etc/GMT-7",
            };
            _WindowsToOlson["Singapore Standard Time"] = new[]
            {
                "Asia/Singapore",
                "Asia/Brunei",
                "Asia/Kuala_Lumpur",
                "Asia/Kuching",
                "Asia/Makassar",
                "Asia/Manila",
                "Etc/GMT-8",
            };
            _WindowsToOlson["South Africa Standard Time"] = new[]
            {
                "Africa/Lusaka",
                "Africa/Blantyre",
                "Africa/Bujumbura",
                "Africa/Gaborone",
                "Africa/Harare",
                "Africa/Johannesburg",
                "Africa/Kigali",
                "Africa/Lubumbashi",
                "Africa/Maputo",
                "Africa/Maseru",
                "Africa/Mbabane",
                "Etc/GMT-2",
            };
            _WindowsToOlson["Sri Lanka Standard Time"] = new[]
            {
                "Asia/Colombo",
            };
            _WindowsToOlson["Syria Standard Time"] = new[]
            {
                "Asia/Damascus",
            };
            _WindowsToOlson["Taipei Standard Time"] = new[]
            {
                "Asia/Taipei",
            };
            _WindowsToOlson["Tasmania Standard Time"] = new[]
            {
                "Australia/Hobart",
                "Australia/Currie",
            };
            _WindowsToOlson["Tokyo Standard Time"] = new[]
            {
                "Asia/Tokyo",
                "Asia/Dili",
                "Asia/Jayapura",
                "Pacific/Palau",
                "Etc/GMT-9",
            };
            _WindowsToOlson["Tonga Standard Time"] = new[]
            {
                "Pacific/Tongatapu",
                "Pacific/Enderbury",
                "Pacific/Fakaofo",
                "Etc/GMT-13",
            };
            _WindowsToOlson["Turkey Standard Time"] = new[]
            {
                "Europe/Istanbul",
            };
            _WindowsToOlson["Ulaanbaatar Standard Time"] = new[]
            {
                "Asia/Ulaanbaatar",
                "Asia/Choibalsan",
            };
            _WindowsToOlson["US Eastern Standard Time"] = new[]
            {
                "America/Indianapolis",
                "America/Indiana/Indianapolis",
                "America/Indiana/Marengo",
                "America/Indiana/Vevay",
            };
            _WindowsToOlson["US Mountain Standard Time"] = new[]
            {
                "US/Mountain",
                "America/Creston",
                "America/Dawson_Creek",
                "America/Hermosillo",
                "America/Phoenix",
                "US/Arizona",
                "Etc/GMT+7",
            };
            _WindowsToOlson["UTC"] = new[]
            {
                "America/Danmarkshavn",
                "Etc/GMT",
            };
            _WindowsToOlson["UTC+12"] = new[]
            {
                "Pacific/Tarawa",
                "Pacific/Funafuti",
                "Pacific/Kwajalein",
                "Pacific/Majuro",
                "Pacific/Nauru",
                "Pacific/Wake",
                "Pacific/Wallis",
                "Etc/GMT-12",
            };
            _WindowsToOlson["UTC-02"] = new[]
            {
                "America/Noronha",
                "Atlantic/South_Georgia",
                "Etc/GMT+2",
            };
            _WindowsToOlson["UTC-11"] = new[]
            {
                "Pacific/Pago_Pago",
                "Pacific/Midway",
                "Pacific/Niue",
                "Etc/GMT+11",
            };
            _WindowsToOlson["Venezuela Standard Time"] = new[]
            {
                "America/Caracas",
            };
            _WindowsToOlson["Vladivostok Standard Time"] = new[]
            {
                "Asia/Vladivostok",
                "Asia/Sakhalin",
            };
            _WindowsToOlson["W. Australia Standard Time"] = new[]
            {
                "Australia/Perth",
                "Antarctica/Casey",
            };
            _WindowsToOlson["W. Central Africa Standard Time"] = new[]
            {
                "Africa/Kinshasa",
                "Africa/Algiers",
                "Africa/Bangui",
                "Africa/Brazzaville",
                "Africa/Douala",
                "Africa/Lagos",
                "Africa/Libreville",
                "Africa/Luanda",
                "Africa/Malabo",
                "Africa/Ndjamena",
                "Africa/Niamey",
                "Africa/Porto-Novo",
                "Africa/Tunis",
                "Etc/GMT-1",
            };
            _WindowsToOlson["W. Europe Standard Time"] = new[]
            {
                "Europe/Berlin",
                "Africa/Tripoli",
                "Arctic/Longyearbyen",
                "Europe/Amsterdam",
                "Europe/Andorra",
                "Europe/Gibraltar",
                "Europe/Luxembourg",
                "Europe/Malta",
                "Europe/Monaco",
                "Europe/Oslo",
                "Europe/Rome",
                "Europe/San_Marino",
                "Europe/Stockholm",
                "Europe/Vaduz",
                "Europe/Vatican",
                "Europe/Vienna",
                "Europe/Zurich",
            };
            _WindowsToOlson["West Asia Standard Time"] = new[]
            {
                "Asia/Tashkent",
                "Antarctica/Mawson",
                "Asia/Aqtau",
                "Asia/Aqtobe",
                "Asia/Ashgabat",
                "Asia/Dushanbe",
                "Asia/Oral",
                "Asia/Samarkand",
                "Indian/Kerguelen",
                "Indian/Maldives",
                "Etc/GMT-5",
            };
            _WindowsToOlson["West Pacific Standard Time"] = new[]
            {
                "Pacific/Port_Moresby",
                "Antarctica/DumontDUrville",
                "Pacific/Guam",
                "Pacific/Saipan",
                "Pacific/Truk",
                "Etc/GMT-10",
            };
            _WindowsToOlson["Yakutsk Standard Time"] = new[]
            {
                "Asia/Yakutsk",
            };

            _OlsonToWindows = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var kvp in _WindowsToOlson)
            {
                foreach (var olson in kvp.Value)
                {
                    _OlsonToWindows[olson] = kvp.Key;
                }
            }

            // These are deprecated or have two windows-id to one Olson
            _WindowsToOlson["Mid-Atlantic Standard Time"] = new[]
            {
                "America/Noronha",
                "Atlantic/South_Georgia",
                "Etc/GMT+2",
            };
            _WindowsToOlson["Belarus Standard Time"] = new[]
            {
                "Europe/Minsk",
            };
            _WindowsToOlson["Libya Standard Time"] = new[]
            {
                "Africa/Tripoli"
            };

            _WindowsToOlson["Russia Time Zone 3"] = new[]
            {
                "Europe/Moscow",
                "Europe/Samara",
                "Europe/Volgograd",
            };
            _WindowsToOlson["Russia Time Zone 10"] = new[]
            {
                "Etc/GMT-11",
            };
            _WindowsToOlson["Russia Time Zone 11"] = new[]
            {
                "Asia/Kamchatka",
                "Asia/Anadyr",
                "Asia/Magadan",
            };
            _WindowsToOlson["Kamchatka Standard Time"] = new[]
            {
                "Asia/Kamchatka",
            };







        }
        public static string ToOlsonTimeZone(this TimeZoneInfo tzi)
        {
            var olsonIds = ToOlsonTimeZones(tzi);
            if (olsonIds != null && olsonIds.Length > 0)
                return olsonIds[0];

            return null;
        }

        public static string[] ToOlsonTimeZones(this TimeZoneInfo tzi)
        {
            if (tzi == null)
                return null;

            string[] olsonIds = null;
            if (_WindowsToOlson.TryGetValue(tzi.Id, out olsonIds) && olsonIds.Length > 0)
                return olsonIds;

            return null;
        }

        public static string FromOlsonToTimeZoneId(string olsonTimezone)
        {
            if (string.IsNullOrWhiteSpace(olsonTimezone))
                return null;

            string id = null;
            if (_OlsonToWindows.TryGetValue(olsonTimezone, out id))
                return id;

            // Try by prefix now
            foreach (var kvp in _OlsonToWindows)
            {
                if (olsonTimezone.StartsWith(kvp.Key))
                    return kvp.Value;
            }

            return null;
        }

        public static TimeZoneInfo FromOlsonToTimeZoneInfo(string olsonTimezone)
        {
            string id = FromOlsonToTimeZoneId(olsonTimezone);
            if (id == null)
                return null;

            return TimeZoneInfo.FindSystemTimeZoneById(id);
        }
    }
}

