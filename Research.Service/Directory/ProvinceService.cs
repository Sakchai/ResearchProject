using System;
using System.Collections.Generic;
using System.Linq;
using Research.Core.Caching;
using Research.Data;
using Research.Services.Events;

namespace Research.Services.Directory
{
    /// <summary>
    ///  province service
    /// </summary>
    public partial class ProvinceService : IProvinceService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Province> _provinceRepository;

        #endregion

        #region Ctor

        public ProvinceService(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IRepository<Province> provinceRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._provinceRepository = provinceRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Deletes a state/province
        /// </summary>
        /// <param name="province">The state/province</param>
        public virtual void DeleteProvince(Province province)
        {
            if (province == null)
                throw new ArgumentNullException(nameof(province));

            _provinceRepository.Delete(province);

            _cacheManager.RemoveByPattern(ResearchDirectoryDefaults.ProvincesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(province);
        }

        /// <summary>
        /// Gets a state/province
        /// </summary>
        /// <param name="provinceId">The state/province identifier</param>
        /// <returns>/province</returns>
        public virtual Province GetProvinceById(int provinceId)
        {
            if (provinceId == 0)
                return null;

            return _provinceRepository.GetById(provinceId);
        }

        /// <summary>
        /// Gets a state/province by abbreviation
        /// </summary>
        /// <param name="abbreviation">The state/province abbreviation</param>
        /// <param name="countryId">Country identifier; pass null to load the state regardless of a country</param>
        /// <returns>/province</returns>
        public virtual Province GetProvinceByAbbreviation(string abbreviation, int? countryId = null)
        {
            if (string.IsNullOrEmpty(abbreviation))
                return null;

            var query = _provinceRepository.Table.Where(state => state.Abbreviation == abbreviation);

            //filter by country

            var province = query.FirstOrDefault();
            return province;
        }

        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <param name="languageId">Language identifier. It's used to sort states by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>s</returns>
        public virtual IList<Province> GetProvincesByCountryId(int countryId, int languageId = 0, bool showHidden = false)
        {
            var key = string.Format(ResearchDirectoryDefaults.ProvincesAllCacheKey, countryId, languageId, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = from sp in _provinceRepository.Table
                            orderby sp.DisplayOrder, sp.Name
                            where (showHidden || sp.Published)
                            select sp;
                var provinces = query.ToList();

                if (languageId > 0)
                {
                    //we should sort states by localized names when they have the same display order
                    provinces = provinces
                        .OrderBy(c => c.DisplayOrder)
                        //.ThenBy(c => _localizationService.GetLocalized(c, x => x.Name, languageId))
                        .ToList();
                }

                return provinces;
            });
        }

        /// <summary>
        /// Gets all states/provinces
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>s</returns>
        public virtual IList<Province> GetProvinces(bool showHidden = false)
        {
            var query = _provinceRepository.Table;
                        //orderby sp.Name
                        //where showHidden || sp.Published
                        //select sp;
            //ar provinces = query.ToList();
            //var provinces = query.ToList();
            return query.ToList();
        }

        /// <summary>
        /// Inserts a state/province
        /// </summary>
        /// <param name="province">/province</param>
        public virtual void InsertProvince(Province province)
        {
            if (province == null)
                throw new ArgumentNullException(nameof(province));

            _provinceRepository.Insert(province);

            _cacheManager.RemoveByPattern(ResearchDirectoryDefaults.ProvincesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(province);
        }

        /// <summary>
        /// Updates a state/province
        /// </summary>
        /// <param name="province">/province</param>
        public virtual void UpdateProvince(Province province)
        {
            if (province == null)
                throw new ArgumentNullException(nameof(province));

            _provinceRepository.Update(province);

            _cacheManager.RemoveByPattern(ResearchDirectoryDefaults.ProvincesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(province);
        }

        #endregion
    }
}