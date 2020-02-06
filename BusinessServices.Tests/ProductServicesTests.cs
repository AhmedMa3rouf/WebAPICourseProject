using DataModel;
using DataModel.GenericRepository;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestHelper;
using Moq;

namespace BusinessServices.Tests
{
    [TestFixture]
    public class ProductServicesTests
    {
        #region Variables
        private IProductServices _productService;
        private IUnitOfWork _unitOfWork;
        private List<Product> _products;
        private GenericRepository<Product> _productRepository;
        private WebApiDbEntities _dbEntities;
        #endregion

        #region Test fixture Setup
        [OneTimeSetUp]
        public void SetUp()
        {
            _products = SetUpProducts();
        }
        #endregion

        #region Test Tear Down
        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
            _products = null;
        }
        #endregion

        #region Setup
        /// <summary>
        /// Re initilaize test
        /// </summary>
        [SetUp]
        public void ReInitializeTest()
        {
            _dbEntities = new Mock<WebApiDbEntities>().Object;
            _productRepository = SetUpProductRepository();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(s => s.ProductRepository).Returns(_productRepository);
            _unitOfWork = unitOfWork.Object;
            _productService = new ProductServices(_unitOfWork);
        }

        /// <summary>
        /// Tear Down all test data
        /// </summary>
        [TearDown]
        public void DisposeTest()
        {
            _productService = null;
            _unitOfWork = null;
            _productRepository = null;
            if (_dbEntities != null)
                _dbEntities.Dispose();
        }
        #endregion


        static List<Product> SetUpProducts()
        {
            var prodID = new int();
            var products = DataInitializer.GetAllProducts();
            foreach (Product product in products)
                product.ProductId = ++prodID;
            return products;
        }

        GenericRepository<Product> SetUpProductRepository()
        {
            // Initialise repository
            var mockRepo = new Mock<GenericRepository<Product>>(MockBehavior.Default, _dbEntities);
            // Setup mocking behavior
            mockRepo.Setup(p => p.GetAll()).Returns(_products);
            mockRepo.Setup(p => p.GetByID(It.IsAny<int>()))
            .Returns(new Func<int, Product>(
            id => _products.Find(p => p.ProductId.Equals(id))));
            mockRepo.Setup(p => p.Insert((It.IsAny<Product>())))
            .Callback(new Action<Product>(newProduct =>
            {
                dynamic maxProductID = _products.Last().ProductId;
                dynamic nextProductID = maxProductID + 1;
                newProduct.ProductId = nextProductID;
                _products.Add(newProduct);
            }));

            mockRepo.Setup(p => p.Update(It.IsAny<Product>()))
                .Callback(new Action<Product>(prod =>
                {
                    var oldProduct = _products.Find(a => a.ProductId == prod.ProductId);
                    oldProduct = prod;
                }));

            mockRepo.Setup(p => p.Delete(It.IsAny<Product>()))
            .Callback(new Action<Product>(prod =>
            {
                var productToRemove = _products.Find(a => a.ProductId == prod.ProductId); if (productToRemove != null)
                    _products.Remove(productToRemove);
            }));
            // Return mock implementation object
            return mockRepo.Object;
        }
    }
}
