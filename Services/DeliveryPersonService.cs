using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BikeRentalApp.Services;

public class DeliveryPersonService : IDeliveryPersonService
{
    private readonly List<DeliveryPerson> _deliveryPersons = new();

    public DeliveryPerson AddDeliveryPerson(DeliveryPerson deliveryPerson)
    {
        _deliveryPersons.Add(deliveryPerson);
        return deliveryPerson;
    }

    public DeliveryPerson GetDeliveryPersonById(Guid id)
    {
        return _deliveryPersons.FirstOrDefault(dp => dp.Id == id);
    }

    public DeliveryPerson GetDeliveryPersonByCnpj(string cnpj)
    {
        return _deliveryPersons.FirstOrDefault(dp => dp.Cnpj == cnpj);
    }

    public DeliveryPerson UpdateDeliveryPerson(DeliveryPerson updatedDeliveryPerson)
    {
        var deliveryPerson = _deliveryPersons.FirstOrDefault(dp => dp.Id == updatedDeliveryPerson.Id);
        if (deliveryPerson != null)
        {
            deliveryPerson.Name = updatedDeliveryPerson.Name;
            deliveryPerson.Cnpj = updatedDeliveryPerson.Cnpj;
            deliveryPerson.BirthDate = updatedDeliveryPerson.BirthDate;
            deliveryPerson.CnhNumber = updatedDeliveryPerson.CnhNumber;
            deliveryPerson.CnhType = updatedDeliveryPerson.CnhType;
            return deliveryPerson;
        }
        return null;
    }

    public bool SaveCnhImage(Guid id, byte[] imageBytes, string fileName)
    {
        var _imageDirectory = "cnh-images";
        var deliveryPerson = _deliveryPersons.FirstOrDefault(dp => dp.Id == id);
        if (deliveryPerson != null)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            if (extension != ".png" && extension != ".bmp")
            {
                return false; // Invalid file type
            }

            string filePath = Path.Combine(_imageDirectory, fileName);

            try
            {
                // Ensure the directory exists
                Directory.CreateDirectory(_imageDirectory);

                // Save the file
                File.WriteAllBytes(filePath, imageBytes);

                // Update the delivery person record with the file name
                deliveryPerson.CnhImageFileName = fileName;

                return true;
            }
            catch
            {
                return false; // Error saving the file
            }
        }
        return false; // Delivery person not found
    }

    public bool DeleteDeliveryPerson(Guid id)
    {
        var deliveryPerson = _deliveryPersons.FirstOrDefault(dp => dp.Id == id);
        if (deliveryPerson != null)
        {
            _deliveryPersons.Remove(deliveryPerson);
            return true;
        }
        return false;
    }
}