using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;

namespace CE_API_Test.MapperTests;

public class ManualMapperTests
{ 
    [Test]
    public void ToBiomarkerOrderModels_GivenBiomarkerOrder_ReturnsMappedBiomarkerOrderModelList()
    {
        //Arrange
        var orderList = GetBiomarkerOrderTuple();
        var biomarkerOrder = GetBiomarkerOrder(orderList);

        //Act
        var biomarkerOrderModels = ManualMapper.ToBiomarkerOrderModels(biomarkerOrder);

        //Assert
        biomarkerOrderModels.Should().NotBeNull();

        foreach (var ordering in orderList)
        {
            AssertBiomarkerOrderProperties(biomarkerOrderModels, ordering);
        }
    }

    [Test]
    public void ToBiomarkerOrder_GivenBiomarkerOrderModelList_ReturnsMappedBiomarkerOrder()
    {
        //Arrange
        var alatOrderModel = CreateBiomarkerOrderModel("alat", 22);
        var weightOrderModel = CreateBiomarkerOrderModel("weight", 7);

        var biomarkerOrderModelList = new List<BiomarkerOrderModel>()
        {
            alatOrderModel, 
            weightOrderModel
        };

        //Act
        var biomarkerOrder = ManualMapper.ToBiomarkerOrder(biomarkerOrderModelList);

        //Assert
        biomarkerOrder.Should().NotBeNull();

        var properties = biomarkerOrder.GetType().GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(biomarkerOrder);
            if (property.Name.ToLower() == alatOrderModel.BiomarkerId)
            {
                ((BiomarkerOrderEntry)value!).OrderNumber.Should().Be(alatOrderModel.OrderNumber);
            }

            if (property.Name.ToLower() == weightOrderModel.BiomarkerId)
            {
                ((BiomarkerOrderEntry)value!).OrderNumber.Should().Be(weightOrderModel.OrderNumber); }
        }
    }

    private BiomarkerOrderModel CreateBiomarkerOrderModel(string biomarkerId, int orderNumber)
    {
        return new()
        {
            BiomarkerId = biomarkerId,
            OrderNumber = orderNumber,
            User = new UserModel() { UserId = "MockUserModel1Id" },
            UserId = "MockUser1Id",
            PreferredUnit = "si"
        };
    }

    private List<(string, int)> GetBiomarkerOrderTuple()
    {
        return new()
        {
            ( "prior_CAD", 5 ),
            ( "Age", 4 ),
            ( "Sex", 3 ),
            ( "Height", 2 ),
            ( "Weight", 1 ),
            ( "ChestPain", 0 ),
            ( "NicotineConsumption", 6 ),
            ( "Diabetes", 7 ),
            ( "CholesterolLowering_Statin", 8 ),
            ( "TCAggregationInhibitor", 9 ),
            ( "ACEInhibitor", 10 ),
            ( "CaAntagonist", 15 ),
            ( "Betablocker", 14 ),
            ( "Diuretic", 13 ),
            ( "OrganicNitrate", 12 ),
            ( "SystolicBloodPressure", 11 ),
            ( "DiastolicBloodPressure", 20 ),
            ( "RestingECG", 19 ),
            ( "PancreaticAmylase", 18 ),
            ( "AlkalinePhosphatase", 17 ),
            ( "HsTroponinT", 16 ),
            ( "Alat", 21 ),
            ( "GlucoseFasting", 24 ),
            ( "Bilirubin", 23 ),
            ( "Urea", 22 ),
            ( "UricAcid", 27 ),
            ( "Cholesterol", 26 ),
            ( "Hdl", 25 ),
            ( "Ldl", 28 ),
            ( "Protein", 29 ),
            ( "Albumin", 30 ),
            ( "Leukocytes", 32 ),
            ( "Mchc", 31 ),
        };
    }

    private void AssertBiomarkerOrderProperties(List<BiomarkerOrderModel> biomarkerOrderModel, (string BiomarkerName, int BiomarkerOrdering) expectedOrderung)
    {
        biomarkerOrderModel.Find(x => x.BiomarkerId == expectedOrderung.BiomarkerName)?.OrderNumber
            .Should().Be(expectedOrderung.BiomarkerOrdering);
    }

    private BiomarkerOrder GetBiomarkerOrder(List<(string BiomarkerName, int BiomarkerOrdering)> expectedOrderList)
    {
        var biomarkerOrder = new BiomarkerOrder();
        var properties = typeof(BiomarkerOrder).GetProperties();

        foreach (var property in properties)
        {
            var value = expectedOrderList.First(x => x.BiomarkerName.Equals(property.Name));
            var newBiomarkerOrderEntry = new BiomarkerOrderEntry() { OrderNumber = value.BiomarkerOrdering };

            property.SetValue(biomarkerOrder, newBiomarkerOrderEntry);
        }

        return biomarkerOrder;
    }
}