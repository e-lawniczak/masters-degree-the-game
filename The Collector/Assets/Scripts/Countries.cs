﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countries : MonoBehaviour
{
    // source https://gist.github.com/keeguon/2310008
    public static List<Country> countries = new List<Country>(new Country[] {
        new Country { Name= "Afghanistan", Code= "AF"},
        new Country { Name= "Åland Islands", Code= "AX"},
        new Country { Name= "Albania", Code= "AL"},
        new Country { Name= "Algeria", Code= "DZ"},
        new Country { Name= "American Samoa", Code= "AS"},
        new Country { Name= "AndorrA", Code= "AD"},
        new Country { Name= "Angola", Code= "AO"},
        new Country { Name= "Anguilla", Code= "AI"},
        new Country { Name= "Antarctica", Code= "AQ"},
        new Country { Name= "Antigua and Barbuda", Code= "AG"},
        new Country { Name= "Argentina", Code= "AR"},
        new Country { Name= "Armenia", Code= "AM"},
        new Country { Name= "Aruba", Code= "AW"},
        new Country { Name= "Australia", Code= "AU"},
        new Country { Name= "Austria", Code= "AT"},
        new Country { Name= "Azerbaijan", Code= "AZ"},
        new Country { Name= "Bahamas", Code= "BS"},
        new Country { Name= "Bahrain", Code= "BH"},
        new Country { Name= "Bangladesh", Code= "BD"},
        new Country { Name= "Barbados", Code= "BB"},
        new Country { Name= "Belarus", Code= "BY"},
        new Country { Name= "Belgium", Code= "BE"},
        new Country { Name= "Belize", Code= "BZ"},
        new Country { Name= "Benin", Code= "BJ"},
        new Country { Name= "Bermuda", Code= "BM"},
        new Country { Name= "Bhutan", Code= "BT"},
        new Country { Name= "Bolivia", Code= "BO"},
        new Country { Name= "Bosnia and Herzegovina", Code= "BA"},
        new Country { Name= "Botswana", Code= "BW"},
        new Country { Name= "Bouvet Island", Code= "BV"},
        new Country { Name= "Brazil", Code= "BR"},
        new Country { Name= "British Indian Ocean Territory", Code= "IO"},
        new Country { Name= "Brunei Darussalam", Code= "BN"},
        new Country { Name= "Bulgaria", Code= "BG"},
        new Country { Name= "Burkina Faso", Code= "BF"},
        new Country { Name= "Burundi", Code= "BI"},
        new Country { Name= "Cambodia", Code= "KH"},
        new Country { Name= "Cameroon", Code= "CM"},
        new Country { Name= "Canada", Code= "CA"},
        new Country { Name= "Cape Verde", Code= "CV"},
        new Country { Name= "Cayman Islands", Code= "KY"},
        new Country { Name= "Central African Republic", Code= "CF"},
        new Country { Name= "Chad", Code= "TD"},
        new Country { Name= "Chile", Code= "CL"},
        new Country { Name= "China", Code= "CN"},
        new Country { Name= "Christmas Island", Code= "CX"},
        new Country { Name= "Cocos (Keeling) Islands", Code= "CC"},
        new Country { Name= "Colombia", Code= "CO"},
        new Country { Name= "Comoros", Code= "KM"},
        new Country { Name= "Congo", Code= "CG"},
        new Country { Name= "Congo, The Democratic Republic of the", Code= "CD"},
        new Country { Name= "Cook Islands", Code= "CK"},
        new Country { Name= "Costa Rica", Code= "CR"},
        new Country { Name= "Croatia", Code= "HR"},
        new Country { Name= "Cuba", Code= "CU"},
        new Country { Name= "Cyprus", Code= "CY"},
        new Country { Name= "Czech Republic", Code= "CZ"},
        new Country { Name= "Denmark", Code= "DK"},
        new Country { Name= "Djibouti", Code= "DJ"},
        new Country { Name= "Dominica", Code= "DM"},
        new Country { Name= "Dominican Republic", Code= "DO"},
        new Country { Name= "Ecuador", Code= "EC"},
        new Country { Name= "Egypt", Code= "EG"},
        new Country { Name= "El Salvador", Code= "SV"},
        new Country { Name= "Equatorial Guinea", Code= "GQ"},
        new Country { Name= "Eritrea", Code= "ER"},
        new Country { Name= "Estonia", Code= "EE"},
        new Country { Name= "Ethiopia", Code= "ET"},
        new Country { Name= "Falkland Islands (Malvinas)", Code= "FK"},
        new Country { Name= "Faroe Islands", Code= "FO"},
        new Country { Name= "Fiji", Code= "FJ"},
        new Country { Name= "Finland", Code= "FI"},
        new Country { Name= "France", Code= "FR"},
        new Country { Name= "French Guiana", Code= "GF"},
        new Country { Name= "French Polynesia", Code= "PF"},
        new Country { Name= "French Southern Territories", Code= "TF"},
        new Country { Name= "Gabon", Code= "GA"},
        new Country { Name= "Gambia", Code= "GM"},
        new Country { Name= "Georgia", Code= "GE"},
        new Country { Name= "Germany", Code= "DE"},
        new Country { Name= "Ghana", Code= "GH"},
        new Country { Name= "Gibraltar", Code= "GI"},
        new Country { Name= "Greece", Code= "GR"},
        new Country { Name= "Greenland", Code= "GL"},
        new Country { Name= "Grenada", Code= "GD"},
        new Country { Name= "Guadeloupe", Code= "GP"},
        new Country { Name= "Guam", Code= "GU"},
        new Country { Name= "Guatemala", Code= "GT"},
        new Country { Name= "Guernsey", Code= "GG"},
        new Country { Name= "Guinea", Code= "GN"},
        new Country { Name= "Guinea-Bissau", Code= "GW"},
        new Country { Name= "Guyana", Code= "GY"},
        new Country { Name= "Haiti", Code= "HT"},
        new Country { Name= "Heard Island and Mcdonald Islands", Code= "HM"},
        new Country { Name= "Holy See (Vatican City State)", Code= "VA"},
        new Country { Name= "Honduras", Code= "HN"},
        new Country { Name= "Hong Kong", Code= "HK"},
        new Country { Name= "Hungary", Code= "HU"},
        new Country { Name= "Iceland", Code= "IS"},
        new Country { Name= "India", Code= "IN"},
        new Country { Name= "Indonesia", Code= "ID"},
        new Country { Name= "Iran, Islamic Republic Of", Code= "IR"},
        new Country { Name= "Iraq", Code= "IQ"},
        new Country { Name= "Ireland", Code= "IE"},
        new Country { Name= "Isle of Man", Code= "IM"},
        new Country { Name= "Israel", Code= "IL"},
        new Country { Name= "Italy", Code= "IT"},
        new Country { Name= "Jamaica", Code= "JM"},
        new Country { Name= "Japan", Code= "JP"},
        new Country { Name= "Jersey", Code= "JE"},
        new Country { Name= "Jordan", Code= "JO"},
        new Country { Name= "Kazakhstan", Code= "KZ"},
        new Country { Name= "Kenya", Code= "KE"},
        new Country { Name= "Kiribati", Code= "KI"},
        new Country { Name= "Korea, Republic of", Code= "KR"},
        new Country { Name= "Kuwait", Code= "KW"},
        new Country { Name= "Kyrgyzstan", Code= "KG"},
        new Country { Name= "Latvia", Code= "LV"},
        new Country { Name= "Lebanon", Code= "LB"},
        new Country { Name= "Lesotho", Code= "LS"},
        new Country { Name= "Liberia", Code= "LR"},
        new Country { Name= "Libyan Arab Jamahiriya", Code= "LY"},
        new Country { Name= "Liechtenstein", Code= "LI"},
        new Country { Name= "Lithuania", Code= "LT"},
        new Country { Name= "Luxembourg", Code= "LU"},
        new Country { Name= "Macao", Code= "MO"},
        new Country { Name= "North Macedonia", Code= "MK"},
        new Country { Name= "Madagascar", Code= "MG"},
        new Country { Name= "Malawi", Code= "MW"},
        new Country { Name= "Malaysia", Code= "MY"},
        new Country { Name= "Maldives", Code= "MV"},
        new Country { Name= "Mali", Code= "ML"},
        new Country { Name= "Malta", Code= "MT"},
        new Country { Name= "Marshall Islands", Code= "MH"},
        new Country { Name= "Martinique", Code= "MQ"},
        new Country { Name= "Mauritania", Code= "MR"},
        new Country { Name= "Mauritius", Code= "MU"},
        new Country { Name= "Mayotte", Code= "YT"},
        new Country { Name= "Mexico", Code= "MX"},
        new Country { Name= "Micronesia, Federated States of", Code= "FM"},
        new Country { Name= "Moldova, Republic of", Code= "MD"},
        new Country { Name= "Monaco", Code= "MC"},
        new Country { Name= "Mongolia", Code= "MN"},
        new Country { Name= "Montserrat", Code= "MS"},
        new Country { Name= "Morocco", Code= "MA"},
        new Country { Name= "Mozambique", Code= "MZ"},
        new Country { Name= "Myanmar", Code= "MM"},
        new Country { Name= "Namibia", Code= "NA"},
        new Country { Name= "Nauru", Code= "NR"},
        new Country { Name= "Nepal", Code= "NP"},
        new Country { Name= "Netherlands", Code= "NL"},
        new Country { Name= "Netherlands Antilles", Code= "AN"},
        new Country { Name= "New Caledonia", Code= "NC"},
        new Country { Name= "New Zealand", Code= "NZ"},
        new Country { Name= "Nicaragua", Code= "NI"},
        new Country { Name= "Niger", Code= "NE"},
        new Country { Name= "Nigeria", Code= "NG"},
        new Country { Name= "Niue", Code= "NU"},
        new Country { Name= "Norfolk Island", Code= "NF"},
        new Country { Name= "Northern Mariana Islands", Code= "MP"},
        new Country { Name= "Norway", Code= "NO"},
        new Country { Name= "Oman", Code= "OM"},
        new Country { Name= "Pakistan", Code= "PK"},
        new Country { Name= "Palau", Code= "PW"},
        new Country { Name= "Palestinian Territory, Occupied", Code= "PS"},
        new Country { Name= "Panama", Code= "PA"},
        new Country { Name= "Papua New Guinea", Code= "PG"},
        new Country { Name= "Paraguay", Code= "PY"},
        new Country { Name= "Peru", Code= "PE"},
        new Country { Name= "Philippines", Code= "PH"},
        new Country { Name= "Pitcairn Islands", Code= "PN"},
        new Country { Name= "Poland", Code= "PL"},
        new Country { Name= "Portugal", Code= "PT"},
        new Country { Name= "Puerto Rico", Code= "PR"},
        new Country { Name= "Qatar", Code= "QA"},
        new Country { Name= "Reunion", Code= "RE"},
        new Country { Name= "Romania", Code= "RO"},
        new Country { Name= "Russian Federation", Code= "RU"},
        new Country { Name= "Rwanda", Code= "RW"},
        new Country { Name= "Saint Helena", Code= "SH"},
        new Country { Name= "Saint Kitts and Nevis", Code= "KN"},
        new Country { Name= "Saint Lucia", Code= "LC"},
        new Country { Name= "Saint Pierre and Miquelon", Code= "PM"},
        new Country { Name= "Saint Vincent and the Grenadines", Code= "VC"},
        new Country { Name= "Samoa", Code= "WS"},
        new Country { Name= "San Marino", Code= "SM"},
        new Country { Name= "Sao Tome and Principe", Code= "ST"},
        new Country { Name= "Saudi Arabia", Code= "SA"},
        new Country { Name= "Senegal", Code= "SN"},
        new Country { Name= "Serbia and Montenegro", Code= "CS"},
        new Country { Name= "Seychelles", Code= "SC"},
        new Country { Name= "Sierra Leone", Code= "SL"},
        new Country { Name= "Singapore", Code= "SG"},
        new Country { Name= "Slovakia", Code= "SK"},
        new Country { Name= "Slovenia", Code= "SI"},
        new Country { Name= "Solomon Islands", Code= "SB"},
        new Country { Name= "Somalia", Code= "SO"},
        new Country { Name= "South Africa", Code= "ZA"},
        new Country { Name= "South Georgia and the South Sandwich Islands", Code= "GS"},
        new Country { Name= "Spain", Code= "ES"},
        new Country { Name= "Sri Lanka", Code= "LK"},
        new Country { Name= "Sudan", Code= "SD"},
        new Country { Name= "Suriname", Code= "SR"},
        new Country { Name= "Svalbard and Jan Mayen", Code= "SJ"},
        new Country { Name= "Swaziland", Code= "SZ"},
        new Country { Name= "Sweden", Code= "SE"},
        new Country { Name= "Switzerland", Code= "CH"},
        new Country { Name= "Syrian Arab Republic", Code= "SY"},
        new Country { Name= "Taiwan", Code= "TW"},
        new Country { Name= "Tajikistan", Code= "TJ"},
        new Country { Name= "Tanzania, United Republic of", Code= "TZ"},
        new Country { Name= "Thailand", Code= "TH"},
        new Country { Name= "Timor-Leste", Code= "TL"},
        new Country { Name= "Togo", Code= "TG"},
        new Country { Name= "Tokelau", Code= "TK"},
        new Country { Name= "Tonga", Code= "TO"},
        new Country { Name= "Trinidad and Tobago", Code= "TT"},
        new Country { Name= "Tunisia", Code= "TN"},
        new Country { Name= "Turkey", Code= "TR"},
        new Country { Name= "Turkmenistan", Code= "TM"},
        new Country { Name= "Turks and Caicos Islands", Code= "TC"},
        new Country { Name= "Tuvalu", Code= "TV"},
        new Country { Name= "Uganda", Code= "UG"},
        new Country { Name= "Ukraine", Code= "UA"},
        new Country { Name= "United Arab Emirates", Code= "AE"},
        new Country { Name= "United Kingdom", Code= "GB"},
        new Country { Name= "United States", Code= "US"},
        new Country { Name= "United States Minor Outlying Islands", Code= "UM"},
        new Country { Name= "Uruguay", Code= "UY"},
        new Country { Name= "Uzbekistan", Code= "UZ"},
        new Country { Name= "Vanuatu", Code= "VU"},
        new Country { Name= "Venezuela", Code= "VE"},
        new Country { Name= "Vietnam", Code= "VN"},
        new Country { Name= "Virgin Islands, British", Code= "VG"},
        new Country { Name= "Virgin Islands, U.S.", Code= "VI"},
        new Country { Name= "Wallis and Futuna", Code= "WF"},
        new Country { Name= "Western Sahara", Code= "EH"},
        new Country { Name= "Yemen", Code= "YE"},
        new Country { Name= "Zambia", Code= "ZM"},
        new Country { Name= "Zimbabwe",Code= "ZN"},
        new Country { Name= "Other",Code= "OT"}
    });


}
public class Country
{
    public string Name { get; set; }
    public string Code { get; set; }
}
