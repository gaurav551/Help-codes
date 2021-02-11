[HttpPost]
        public async Task<IActionResult> ChangeSessions(int[] items)
        {
            int sectionId;
            string currentYear = DateTime.Now.Year.ToString();
            string json = await System.IO.File.ReadAllTextAsync("applicationSettings.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string year = jsonObj["AcademicSession"]["Year"];
            bool isChanged = jsonObj["AcademicSession"]["IsChanged"];
            if (year == currentYear && isChanged)
            {
                return Json(new { success = true, responseText = "Sorry session already changed this year" });

            }

            try
            {
                foreach (var x in context.Students.ToList())
                {
                    var studentClass = items.FirstOrDefault(p => p == x.Class);
                    sectionId = studentClass;
                    var index = Array.FindIndex(items, x => x == studentClass);
                    System.Console.WriteLine(items.Count());
                    int nextClass = items[index + 1];
                    var sectionsInNextClass = context.Sections.Where(x => x.ClassId == nextClass).Select(x => x.Id).ToList();
                    if (sectionsInNextClass.Count <= 0)
                    {
                        return Json(new { success = true, responseText = "Sorry There is no section in other class" });

                    }

                    if (studentClass == items[items.Length - 1])
                    {
                        x.IsDisabled = true;
                    }
                    else
                    {
                        //Pick random index of list
                        x.Section = sectionsInNextClass.PickRandom();
                        x.Class = nextClass;
                    }
                    context.Students.Update(x);
                }
                context.SaveChanges();
                // foreach (var x in items)
                // {
                //     System.Console.WriteLine(x);
                // }
                await AddToJsonFile();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);

            }

            return Json(new { success = true, responseText = "All students has been incremented to next class" });
        }
        public async Task AddToJsonFile()
        {
            string json = await System.IO.File.ReadAllTextAsync("applicationSettings.json");

            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj["AcademicSession"]["Year"] = DateTime.Now.Year.ToString();
            jsonObj["AcademicSession"]["IsChanged"] = true;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText("applicationSettings.json", output);


        }
