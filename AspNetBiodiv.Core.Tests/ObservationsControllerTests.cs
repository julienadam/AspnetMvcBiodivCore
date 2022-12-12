using AspNetBiodiv.Core.Web.Controllers;
using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AspNetBiodiv.Core.Tests
{
    public class ObservationsControllerTests
    {
        private const string SampleEmail = "foo@bar.org";
        private readonly Mock<IObservations> observations;
        private readonly ObservationsController controller;

        public ObservationsControllerTests()
        {
            observations = new Mock<IObservations>();
            controller = new ObservationsController(new Mock<ITaxonomie>().Object, observations.Object);
        }

        [Fact]
        public void Observation_is_invalid_if_number_of_posts_for_user_today_is_five()
        {
            observations
                .Setup(x => x.NumberOfObservationsToday(SampleEmail))
                .Returns(5);

            var result = controller.ValidateNumberOfPosts(SampleEmail);

            var json = Assert.IsType<JsonResult>(result);
            Assert.IsType<string>(json.Value);
        }

        [Fact]
        public void Observation_is_valid_if_number_of_posts_for_user_today_is_less_than_five()
        {
            observations
                .Setup(x => x.NumberOfObservationsToday(SampleEmail))
                .Returns(4);

            var result = controller.ValidateNumberOfPosts(SampleEmail);

            var json = Assert.IsType<JsonResult>(result);
            Assert.Equal(true, json.Value);
        }
    }
}
