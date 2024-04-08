using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MetricsScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField ageInput;
    [SerializeField] private TMP_Dropdown genderDropdown;
    [SerializeField] private TMP_Dropdown experienceDropdown;
    [SerializeField] private TMP_Dropdown educationDropdown;
    [SerializeField] private TMP_Dropdown countryDropdown;
    [SerializeField] private TMP_Dropdown demographicDropdown;
    [SerializeField] private TextMeshProUGUI errorMsg;
    [SerializeField] private GameObject buttonsToHide;

    private TMP_Dropdown[] dropdowns;

    private Color errorColor = new Color(1f, (float)(121f / 255f), (float)(121f / 255f));
    private readonly string apiUrl = "https://erykmgr.thinq.pl";
    private string Error;


    private void Start()
    {
        FillGenderDD();
        FillExperienceDD();
        FillEducationDD();
        FillCountryDD();
        FillDemographicDD();
        dropdowns = new TMP_Dropdown[] {
            genderDropdown, experienceDropdown, educationDropdown, countryDropdown, demographicDropdown
        };
    }
    public void SubmitMetrics()
    {
        //buttonsToHide.SetActive(false);
        StartCoroutine(SendMetrics());
    }
    IEnumerator SendMetrics()
    {
        var flag = true;
        int age = 0;
        if (ageInput.text.Length <= 0)
        {
            SetErrorMsg("Age cannot be empty");
            flag = false;
        }
        else if (!Int32.TryParse(ageInput.text, out age))
        {
            SetErrorMsg("Age must be a number");
            flag = false;
        }
        else if (dropdowns.Where(d => d.options[d.value].text.Length <= 0).Count() > 0)
        {
            SetErrorMsg("Fill in all the data");
            flag = false;
        }
        FirstLoginMetrics requestObj = new FirstLoginMetrics
        {
            Age = age,
            Gender = genderDropdown.options[genderDropdown.value].text,
            Education = educationDropdown.options[educationDropdown.value].text,
            GamesExperience = experienceDropdown.options[experienceDropdown.value].text,
            Country = countryDropdown.options[countryDropdown.value].text,
            DemographicBackground = demographicDropdown.options[demographicDropdown.value].text,
            UserId = RuntimeVariables.PlayerId
        };

        if (flag)
        {
            UnityWebRequest req = UnityWebRequest.Post(apiUrl + "/api/users/updateMetrics", JsonUtility.ToJson(requestObj), "application/json");
            req.useHttpContinue = false;
            req.SetRequestHeader("Authorization", "Bearer " + RuntimeVariables.PlayerJwtToken);
            yield return req.SendWebRequest();


            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.downloadHandler.text);
                Error = req.downloadHandler.text;
                Debug.LogError(req.error);
                SetErrorMsg("Something went wrong. My bad.");
            }
            else
            {
                OpenScene(SceneNames.Stats);
            }
        }
    }
    public void SetErrorMsg(string msg)
    {
        errorMsg.text = msg;
        errorMsg.color = errorColor;
        buttonsToHide.SetActive(true);
    }
    public void OpenScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    void FillGenderDD()
    {
        genderDropdown.AddOptions(new List<TMP_Dropdown.OptionData>
        (
            new TMP_Dropdown.OptionData[] {
                new TMP_Dropdown.OptionData { text = "Female"},
                new TMP_Dropdown.OptionData { text = "Male"},
                new TMP_Dropdown.OptionData { text = "Other"},
            }
        ));
    }
    void FillExperienceDD()
    {
        experienceDropdown.AddOptions(new List<TMP_Dropdown.OptionData>
        (
            new TMP_Dropdown.OptionData[] {
                new TMP_Dropdown.OptionData { text = "Not experienced (Not playing games at all)"},
                new TMP_Dropdown.OptionData { text = "Somewhat experienced (Have played/Playing one or multiple games from time to time)"},
                new TMP_Dropdown.OptionData { text = "Experienced (Have played/playing one or multiple games on weekly basis)"},
                new TMP_Dropdown.OptionData { text = "Very experienced (Have played/playing one or mulptiple games on daily basis)"},
            }
        ));
    }
    void FillEducationDD()
    {
        educationDropdown.AddOptions(new List<TMP_Dropdown.OptionData>
       (
           new TMP_Dropdown.OptionData[] {
                new TMP_Dropdown.OptionData { text = "No education"},
                new TMP_Dropdown.OptionData { text = "Primary school"},
                new TMP_Dropdown.OptionData { text = "Secondary school"},
                new TMP_Dropdown.OptionData { text = "High school"},
                new TMP_Dropdown.OptionData { text = "College"},
                new TMP_Dropdown.OptionData { text = "University - bachelor's degree or higher"},
           }
       ));
    }
    void FillCountryDD()
    {
        countryDropdown.AddOptions(new List<TMP_Dropdown.OptionData>
      (
         Countries.countries.Select(c => new TMP_Dropdown.OptionData { text = c.Name }).ToArray()
      ));
    }
    void FillDemographicDD()
    {
        demographicDropdown.AddOptions(new List<TMP_Dropdown.OptionData>
     (
         new TMP_Dropdown.OptionData[] {
                new TMP_Dropdown.OptionData { text = "Country/Vilalge"},
                new TMP_Dropdown.OptionData { text = "City up to 50k population "},
                new TMP_Dropdown.OptionData { text = "City from 50k to 150k population "},
                new TMP_Dropdown.OptionData { text = "City from 150k to 500k population "},
                new TMP_Dropdown.OptionData { text = "City from 500k to 1m population "},
                new TMP_Dropdown.OptionData { text = "City above 1m population "},
           }
     ));
    }
    [Serializable]
    public class FirstLoginMetrics
    {
        public string Country;
        public int Age;
        public string Gender;
        public string GamesExperience;
        public string Education;
        public string DemographicBackground;
        public int UserId;
    }
}
