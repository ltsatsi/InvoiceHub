using IMS_DomainLayer.Dtos.Client;
using IMS_DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Mappers
{
    public static class ClientMapper
    {
        public static Client ToClient(this ClientDto clientDto)
        {
            return new Client
            {
                FullName = clientDto.FullName,
                Email = clientDto.Email,
                AddressLine = clientDto.AddressLine,
                IsActive = true,
            };
        }


        public static ClientDto ToClientDto(this Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                ApplicationUserId = client.ApplicationUserId,

                FullName = client.FullName,
                Email = client.Email,
                AddressLine = client.AddressLine,
                Invoices = client.Invoices.Select(i => i.ToInvoiceDto()).ToList(),
            };
        }
    }
}
