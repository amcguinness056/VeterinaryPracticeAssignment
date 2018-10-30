using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace VeterinaryPractice
{
    class VeterinaryMenu
    {
        static void Main(string[] args)
        {
            VeterinaryMenu veterinaryMenu = new VeterinaryMenu();

            using(var db = new VeterinaryModelContainer())
            {
                Console.WriteLine("VETERNINARY PRACTICE ADMIN SYSTEM");

                while (true)
                {
                    Console.Write("(1) - List the Owners registered in the Veterinary Practice \n" +
                                  "(2) - List all the Pets registered in the Veterinary Practice \n" +
                                  "(3) - Given the practice RegNum display details of practice and all vets working there \n" + 
                                  "(4) - Given a PetId, list Name, Type and Breed of Pet followed by date and reason of all visits \n" + 
                                  "(5) - Given a VetId and a specified date, list all visits with that vet \n" + 
                                  "(6) - Given a PetId, calculate the cost of the most recent visit to the vet \n" + 
                                  "(7) - LOGOUT \n");
                    //Handle User selection
                    int sel = 0;
                    Console.WriteLine("ENTER Your Selection: ");
                    while(!(int.TryParse(Console.ReadLine(), out sel) && (sel >= 1 && sel <= 7)))
                    {
                        Console.WriteLine("You need to enter a value between 1-7!");
                        Console.WriteLine("ENTER Your Selection: ");
                    }

                    Console.WriteLine();

                    switch (sel)
                    {
                        case 1:
                            veterinaryMenu.OwnersListing(db);
                            break;
                        case 2:
                            veterinaryMenu.PetListing(db);
                            break;
                        case 3:
                            veterinaryMenu.VetsGivenPracticeRegNum(db);
                            break;
                        case 4:
                            veterinaryMenu.ListPetAndVisitGivenPetId(db);
                            break;
                        case 5:
                            veterinaryMenu.GivenVetIdListPetsAndOwnners(db);
                            break;
                        case 6:
                            veterinaryMenu.GivenPetIdCalculateInvoice(db);
                            break;
                        case 7:
                            Console.WriteLine("LOGGING OUT");
                            break;
                        default:
                            Console.WriteLine("Invalid selection");
                            break;

                    }
             
                }
            }

        }

        //List all Owners that are registered to the practice
        public void OwnersListing(VeterinaryModelContainer db)
        {
            var query = from o in db.Owners
                        orderby o.Surname
                        select o;

            Console.WriteLine("VETERINARY PET OWNERS LISTING");
            foreach(var item in query)
            {
                Console.WriteLine(item.Surname + " " + item.Firstname);
            }
            Console.WriteLine("---------------------------------------");
            
        }

        //List all pets registered to the practice
        public void PetListing(VeterinaryModelContainer db)
        {
            var query = from p in db.Pets
                        orderby p.Name
                        select p;
            Console.WriteLine("VETERINARY PETS LISTING");
            foreach(var item in query)
            {
                Console.WriteLine("Name: " + item.Name + "\nType: " + item.Type + "\nBreed: " + item.Breed + "\n");
            }

            Console.WriteLine("---------------------------------------");
        }

        //Given Practice RegNum list details of practice including vets who work there
        public void VetsGivenPracticeRegNum(VeterinaryModelContainer db)
        {
            string regNumber = getRegNumber();

            bool regNumExists = db.Practices.Any(r => r.RegNumber.Equals(regNumber));
            if (regNumExists)
            {
                var queryPracticeDetails = from p in db.Practices
                                           join v in db.Vets on p.Id equals v.PracticeId
                                           where p.RegNumber.Equals(regNumber)
                                           select new
                                           {
                                               practice = p,
                                               vetDetails = v
                                           };

              
                    Console.WriteLine("DETAILS OF PRACTICE WITH REGNUM " + regNumber);
                    Console.WriteLine(queryPracticeDetails.FirstOrDefault().practice.PracticeName + " - " + queryPracticeDetails.FirstOrDefault().practice.Address + "\nVETS WORKING AT THIS PRACTICE:");
                    foreach (var item in queryPracticeDetails)
                    {
                       
                        Console.WriteLine(item.vetDetails.Firstname + " " + item.vetDetails.Surname);
                    }
                    Console.WriteLine("---------------------------------------");
                
            }
            else
            {
                Console.WriteLine("THAT REGNUM DOES NOT EXIST IN THE RECORDS");
                Console.WriteLine("---------------------------------------");
            }
        }

        public void ListPetAndVisitGivenPetId(VeterinaryModelContainer db)
        {
            int petId = Int32.Parse(getPetId());

            bool petIdExists = db.Pets.Any(r => r.Id.Equals(petId));
            if (petIdExists)
            {
                var query = from p in db.Pets
                            join v in db.Visits on p.Id equals v.PetId
                            where p.Id.Equals(petId)
                            orderby v.VisitDate
                            select new
                            {
                                pets = p,
                                visits = v
                            };
                Console.WriteLine("DETAILS OF PET WITH PETID " + petId);
                Console.WriteLine("Name: " + query.FirstOrDefault().pets.Name +
                                  "\nType: " + query.FirstOrDefault().pets.Type +
                                  "\nBreed: " + query.FirstOrDefault().pets.Breed);
                foreach(var item in query)
                {
                    Console.WriteLine("DETIALS OF VISIT" + "\n---------------");
                    Console.WriteLine("Visit Date: " + item.visits.VisitDate +
                                      "\nVisit Reason: " + item.visits.VisitReason + "\n");
                }
                Console.WriteLine("---------------------------------------");
            }
            else
            {
                Console.WriteLine("THAT PETID DOES NOT EXIST IN THE RECORDS");
                Console.WriteLine("---------------------------------------");
            }
        }

        public void GivenVetIdListPetsAndOwnners(VeterinaryModelContainer db)
        {
            int vetId = Int32.Parse(getVetId());
            bool vetIdExists = db.Vets.Any(r => r.Id.Equals(vetId));

            if (vetIdExists)
            {
                string date = getDate();
                DateTime dateTimeInputed = Convert.ToDateTime(date);
                
                var query = from v in db.Vets
                            join vi in db.Visits on v.Id equals vi.VetId
                            join p in db.Pets on vi.PetId equals p.Id
                            join o in db.Owners on p.OwnerId equals o.Id
                            where v.Id.Equals(vetId)
                            orderby vi.VisitDate
                            select new
                            {
                                vets = v,
                                visits = vi,
                                pets = p,
                                owners = o
                            };

                Console.WriteLine("Appointments for Vet " + query.FirstOrDefault().vets.Firstname + " " + query.FirstOrDefault().vets.Surname + " on date " + date);
                bool noAppointments = false;

                foreach(var item in query)
                {
                    DateTime dateTime = Convert.ToDateTime(item.visits.VisitDate);
                    if(dateTimeInputed.Equals(dateTime))
                    {
                        Console.WriteLine("DETAILS OF VISIT" + "\n---------------");
                        Console.WriteLine("Name of Pet: " + item.pets.Name);
                        Console.WriteLine("Name of Owner: " + item.owners.Firstname + " " + item.owners.Surname);
                    }
                    else
                    {
                        noAppointments = true;
                    }
                    
                }
                if (noAppointments)
                    Console.WriteLine("NO APPOINTMENTS ON THIS DATE");
                Console.WriteLine("---------------------------------------");

            }
            else
            {
                Console.WriteLine("THAT VETID DOES NOT EXIST IN THE RECORDS");
                Console.WriteLine("---------------------------------------");
            }
        }

        public void GivenPetIdCalculateInvoice(VeterinaryModelContainer db)
        {
            int petId = Int32.Parse(getPetId());
            bool petIdExists = db.Pets.Any(r => r.Id.Equals(petId));

            if (petIdExists)
            {
                decimal labourRate = 40;
                var query = from vi in db.Visits
                            join p in db.Pets on vi.PetId equals p.Id
                            join m in db.Medications on vi.Id equals m.VisitId
                            where vi.PetId.Equals(petId)
                            orderby vi.VisitDate
                            select new
                            {
                                visits = vi,
                                pets = p,
                                medication = m
                            };
                if (query.Any())
                {
                    Console.WriteLine("DETAILS OF SERVICE");
                    Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++");
                    Console.WriteLine(query.FirstOrDefault().pets.Name + " Last visited on " + query.FirstOrDefault().visits.VisitDate);
                    decimal totalMedicationCost = 0;
                    foreach (var item in query)
                    {
                        decimal medicationPrice = decimal.Parse(item.medication.MedicationPrice);
                        int medicationQty = Int32.Parse(item.medication.MedicationQty);
                        decimal medicationCost = medicationPrice * medicationQty;
                        totalMedicationCost += medicationCost;
                        Console.WriteLine("Medication Issued: " + item.medication.MedicationName);
                        Console.WriteLine("Medication Price: £" + medicationPrice);
                        Console.WriteLine("Medication Qty: " + item.medication.MedicationQty);
                        Console.WriteLine("---------------------------------");

                    }
                    decimal labourCost = decimal.Parse(query.FirstOrDefault().visits.DurationHours) * labourRate;
                    decimal totalCost = totalMedicationCost + labourCost;
                    Console.WriteLine("Total Medication Cost: £" + totalMedicationCost);
                    Console.WriteLine("Labour Time: " + query.FirstOrDefault().visits.DurationHours + "hrs");
                    Console.WriteLine("Labour Cost: £" + labourCost);
                    Console.WriteLine("Total Cost: £" + totalCost);
                    Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++");
                }
                else
                {
                    Console.WriteLine("NO VISITS IN RECORDS");
                }
                
            }
            else
            {
                Console.WriteLine("PETID DOES NOT EXIST");
            }
        }



        //--------------------UTILITY METHODS-------------------
        string getInput(string msg, string regExpression)
        {
            Console.Write(msg);
            string input = Console.ReadLine();
            
            //check the format of the the input
            while (!Regex.IsMatch(input, regExpression))
            {
                Console.WriteLine("Invalid entry");
                Console.Write(msg);
                input = Console.ReadLine();
            }
            return input;

        }

        string getRegNumber()
        {
            return getInput("Enter VETERINARY PRACTICE REG NUMBER: ", @"^[0-9]+$");
        }

        string getPetId()
        {
            return getInput("Enter PETID: ", @"^[0-9]+$");
        }


        string getVetId()
        {
            return getInput("Enter VETID: ", @"^[0-9]+$");
        }

        string getDate()
        {
            return getInput("Enter DATE in format dd/mm/yyyy, dd.mm.yyyy or dd-mm-yyyy: ", @"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
        }

    }
}
