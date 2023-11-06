using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace PetApiTest
{
    public class PetsApiTest
    {
        private HttpClient httpClient;

        public PetsApiTest()
        {
            WebApplicationFactory<Program> webApplicationFactory = new WebApplicationFactory<Program>();
            this.httpClient = webApplicationFactory.CreateClient();
        }
        [Fact]
        public async void Should_return_pet_created_with_status_201_when_create_given_a_JSON_pet()
        {
            // Given
            Pet petGiven = new Pet("Snowball", "Cat", "White", 99);
            // When
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("/api/pets", petGiven);
            // Then
            Pet petCreated = await httpResponseMessage.Content.ReadFromJsonAsync<Pet>();
            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            Assert.Equal(petGiven, petCreated);
        }

        [Fact]
        public async void Should_return_bad_request_when_create_given_existing_JSON_pet()
        {
            // Given
            await httpClient.DeleteAsync("api/pets");
            Pet petGiven = new Pet("Snowball", "Cat", "White", 99);
            await httpClient.PostAsJsonAsync("api/pets", petGiven);
            // When
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("api/pets", petGiven);

            // Then
            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }
    }
}