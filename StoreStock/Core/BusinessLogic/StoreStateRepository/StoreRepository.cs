﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreStock.Models;

namespace StoreStock.BusinessLogic {
  class StoreRepository : IStoreRepository{
    private IFactory _factory;
    private IStockRepository _repository;
    private IStoreState _init;
    private IStoreState _run;
    private IStoreState _stop;
    private bool _isInitialized = false;
    private IStoreState _state;
    internal StoreRepository(IFactory factory, IStockRepository repository) {
      _factory = factory;
      _repository = repository;
      // _state = _init;
    }

    Store IStoreRepository.ReadStore() {
      if (_state != null) {
        return _state.ReadStore();
      }
      else {
        return null;
      }
    }

    void IStoreRepository.UpdateStore(string name) {
      if (_state!= null) {
        _state.UpdateStore(name);
      }
      else {

      }
    }
    bool IStoreRepository.Init() {
      if (_init == null && !_isInitialized) {
        _init = new StoreRepository_Init(_factory, _repository);
        _isInitialized = true;
      }
      _state = _init;
    }
    bool IStoreRepository.Run() {
      if (_run == null) {
        _run = new StoreRepository_Run(_factory, _repository);
      }
      _state = _run;
    }
    bool IStoreRepository.Stop() {
      if (_stop == null) {
        _stop = new StoreRepository_Stop(_factory, _repository);
      }
      _state = _stop;
    }
    internal IStoreState GetInitState() {
      return _init;
    }
    internal IStoreState GetRunState() {
      return _run;
    }
    internal IStoreState GetShutDownState() {
      return _stop;
    }
  }
}
