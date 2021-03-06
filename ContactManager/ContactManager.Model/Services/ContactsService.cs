﻿using ContactManager.Data;
using ContactManager.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Model.Services
{
    public class ContactsService
    {
        public static List<ContactModel> GetContacts()
        { //vraca sve kontake za prikaz
            using (var context = new ContactManagerDBEntities())
            {
                List<ContactModel> contacts = new List<ContactModel>();

                foreach (var c in context.Contacts)
                {
                    var type = context.ConactTypes.Where(x => x.ContactTypeID == c.ContactTypeID).First();

                    contacts.Add(new ContactModel
                    {
                        Phone = c.Phone,
                        Address = c.Address,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        InsertDate = c.InsertDate,
                        ContactID = c.ContactID,
                        ContactTypeID = c.ContactTypeID,
                        ContactType = type.Caption
                    });
                }

                return contacts;
            }
        }

        public static void DeleteContact(ContactModel c)
        { //brise 1 kontakt iz baze
            using (var context = new ContactManagerDBEntities())
            {
                var toDelete = context.Contacts.Where(x => x.ContactID == c.ContactID).First();
                context.Contacts.Remove(toDelete);

                context.SaveChanges();
            }
        }

        public static void DeleteAllContacts()
        { //brise sve kontakte iz baze
            using (var context = new ContactManagerDBEntities())
            {
                context.Contacts.RemoveRange(context.Contacts);

                context.SaveChanges();
            }
        }

        public static void AddContact(ContactModel c)
        { //dodaje kontakt u bazu
            using (var context = new ContactManagerDBEntities())
            {
                Contact toAdd = new Contact
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Phone = c.Phone,
                    Address = c.Address,
                    ContactTypeID = c.ContactTypeID,
                    InsertDate = c.InsertDate
                };
                context.Contacts.Add(toAdd);

                context.SaveChanges();
            }
        }

        public static void ImportContacts(List<ContactModel> contacts)
        { //preuzima listu kontakta ucitanu iz fajla u upisuje ih u bazu
            using (var context = new ContactManagerDBEntities())
            {
                List<Contact> toAdd = new List<Contact>();
                foreach (var c in contacts)
                {
                    Contact newContact = new Contact
                    {
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Phone = c.Phone,
                        Address = c.Address,
                        ContactTypeID = c.ContactTypeID,
                        InsertDate = c.InsertDate
                    };

                    toAdd.Add(newContact);
                }
                
                context.Contacts.AddRange(toAdd);

                context.SaveChanges();
            }
        }

        public static void UpdateContact(ContactModel c)
        { //azuriranje kontakta
            using (var context = new ContactManagerDBEntities())
            {
                Contact toUpdate = context.Contacts.Where(x => x.ContactID == c.ContactID).FirstOrDefault();

                toUpdate.FirstName = c.FirstName;
                toUpdate.LastName = c.LastName;
                toUpdate.Address = c.Address;
                toUpdate.ContactTypeID = c.ContactTypeID;
                toUpdate.InsertDate = c.InsertDate;
                toUpdate.Phone = c.Phone;

                context.SaveChanges();
            }
        }
    }
}
