using Sitecore.ExperienceForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Feature.FormsExtensions.Fields.FileUpload;

namespace AIEnterprise.Feature.Forms.DataDictionary
{
    public class FormDictionary
    {
        public Dictionary<string, List<string>> hstable = new Dictionary<string, List<string>>();

        public Dictionary<string, List<string>> GetFieldsDictionary(IList<IViewModel> fields)
        {
            var fieldDescriptors = new List<string>();
            foreach (var field in fields)
            {
                if (field is FileUploadModel)
                {
                    var fileUpload = field as FileUploadModel;
                    if (!String.IsNullOrEmpty(fileUpload.Name) && !hstable.ContainsKey(fileUpload.Name))
                    {
                        // alreadyExist = hstable.Where(i => i.Key.Equals(stringField.Name)).FirstOrDefault();                        
                        hstable.Add(fileUpload.Name, new List<string>() { fileUpload.Value.Url });

                    }
                }
                if (field is StringInputViewModel)
                {
                    var stringField = field as StringInputViewModel;
                    if (!String.IsNullOrEmpty(stringField.Name) && !hstable.ContainsKey(stringField.Name))
                    {
                        // alreadyExist = hstable.Where(i => i.Key.Equals(stringField.Name)).FirstOrDefault();                        
                        hstable.Add(stringField.Name, new List<string>() { stringField.Value });

                    }
                }
                if (field is DropDownListViewModel)
                {
                    var stringField = field as DropDownListViewModel;
                    if (!String.IsNullOrEmpty(stringField.Name) && !hstable.Keys.Contains(stringField.Name))
                    {
                        hstable.Add(stringField.Name, new List<string>() { stringField.Value[0] });
                    }
                }
                if (field is CheckBoxListViewModel)
                {
                    var stringField = field as CheckBoxListViewModel;
                    if (!String.IsNullOrEmpty(stringField.Name))
                    {
                        var alreadyExist = hstable.Where(i => i.Key.Equals(stringField.Name)).FirstOrDefault();
                        if (!String.IsNullOrEmpty(alreadyExist.Key))
                        {
                            alreadyExist.Value.Add(stringField.Value[0].ToString());
                            hstable.Add(alreadyExist + "[]", alreadyExist.Value);
                            hstable.Remove(alreadyExist.Key);
                        }
                        else
                        {
                            alreadyExist = hstable.Where(i => i.Key.Equals(stringField.Name + "[]")).FirstOrDefault();
                            if (!String.IsNullOrEmpty(alreadyExist.Key))
                            {
                                alreadyExist.Value.Add(stringField.Value[0].ToString());
                                hstable.Add(alreadyExist + "[]", alreadyExist.Value);
                            }
                            else
                            {
                                hstable.Add(stringField.Name, stringField.Value);
                            }
                        }
                    }
                }
                if (field is CheckBoxViewModel)
                {
                    var stringField = field as CheckBoxViewModel;
                    if (!String.IsNullOrEmpty(stringField.Name))
                    {
                        var alreadyExist = hstable.Where(i => i.Key.Equals(stringField.Name)).FirstOrDefault();
                        if (!String.IsNullOrEmpty(alreadyExist.Key))
                        {
                            alreadyExist.Value.Add(stringField.ToString());
                            hstable.Add(alreadyExist + "[]", alreadyExist.Value);
                            hstable.Remove(alreadyExist.Key);
                        }
                        else
                        {
                            alreadyExist = hstable.Where(i => i.Key.Equals(stringField.Name + "[]")).FirstOrDefault();
                            if (!String.IsNullOrEmpty(alreadyExist.Key))
                            {
                                alreadyExist.Value.Add(stringField.ToString());
                                hstable.Add(alreadyExist + "[]", alreadyExist.Value);
                            }
                            else
                            {
                                hstable.Add(stringField.Name, new List<string>() { stringField.Value.ToString() });
                            }
                        }
                    }
                }
                //checkboxviewmodel\\
                if (field is CheckBoxViewModel)
                {
                    var checkBoxField = field as CheckBoxViewModel;
                    if (checkBoxField.Value)
                    {
                        var alreadyExist = hstable.Where(i => i.Key.Equals(checkBoxField.Name)).FirstOrDefault();
                        if (!String.IsNullOrEmpty(alreadyExist.Key))
                        {
                            alreadyExist.Value.Add(checkBoxField.Title.ToString());
                        }
                        else
                        {
                            if (checkBoxField.Name.Equals("formsEmailOptIn"))
                            {
                                hstable.Add(checkBoxField.Name, new List<string>() { "True" });
                            }
                            else
                            {
                                hstable.Add(checkBoxField.Name, new List<string>() { checkBoxField.Title.ToString() });
                            }
                        }
                    }
                }
            }
            return hstable;
        }
    }
}