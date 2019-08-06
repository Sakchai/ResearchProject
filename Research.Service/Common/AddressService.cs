using Research.Core;
using Research.Core.Caching;
using Research.Data;
using Research.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Research.Services.Common
{
    /// <summary>
    /// Address service    
    /// </summary>
    public partial class AddressService : IAddressService
    {
        #region Fields

        private readonly IRepository<Address> _addressRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="staticCacheManager">Static cache manager</param>
        /// <param name="addressRepository">Address repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public AddressService(ICacheManager cacheManager,
            IStaticCacheManager staticCacheManager,
            IRepository<Address> addressRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._addressRepository = addressRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete address
        /// </summary>
        /// <param name="address">Address</param>
        public virtual void DeleteAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            _addressRepository.Delete(address);

            //event notification
            _eventPublisher.EntityDeleted(address);

        }


        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="addressName">Address name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Addresss</returns>
        public virtual IPagedList<Address> GetAllAddresss(string addressName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _addressRepository.Table;
            if (!string.IsNullOrEmpty(addressName))
                query = query.Where(a => a.Address1 == addressName);

            //paging
            return new PagedList<Address>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a address
        /// </summary>
        /// <param name="addressId">Address identifier</param>
        /// <returns>Address</returns>
        public virtual Address GetAddressById(int addressId)
        {
            if (addressId == 0)
                return null;

            var key = string.Format(ResearchAddressDefaults.AddressesByIdCacheKey, addressId);
            return _cacheManager.Get(key, () => _addressRepository.GetById(addressId));
        }

        /// <summary>
        /// Inserts address
        /// </summary>
        /// <param name="address">Address</param>
        public virtual void InsertAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            if (address is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _addressRepository.Insert(address);

            //cache
            _cacheManager.RemoveByPattern(ResearchAddressDefaults.AddressesPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchAddressDefaults.AddressesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(address);
        }

        /// <summary>
        /// Updates the address
        /// </summary>
        /// <param name="address">Address</param>
        public virtual void UpdateAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            if (address is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _addressRepository.Update(address);

            //cache
            _cacheManager.RemoveByPattern(ResearchAddressDefaults.AddressesPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchAddressDefaults.AddressesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(address);
        }



    



        #endregion
    }
}
