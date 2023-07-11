import { createStore } from 'vuex';
import axios from 'axios';

export default createStore({
  state: {
    budgetSpentData: {},
    budgetBalanceData: {},
    transactionsLoaded: false,
    transactions: [],
  },
  mutations: {
    setBudgetSpentData(state, budgetSpentData) {
      state.budgetSpentData = budgetSpentData;
    },
    setTransactionsLoaded(state, value) {
      state.transactionsLoaded = value;
    },
    setBudgetBalanceData(state, budgetBalanceData) {
      state.budgetBalanceData = budgetBalanceData;
    },
    setTransactions(state, transactions) {
      state.transactions = transactions;
    }
  },
  actions: {
    fetchBudgetSpentData({ commit }) { //state
      axios.get('https://localhost:7042/api/Budget/spent').then(response => {
          if (response.data) {
            commit('setBudgetSpentData', response.data);
          }
        })
        .catch(error => {
          console.error(error);
        });
      },
    fetchBudgetBalanceData({ commit }) {
      axios.get('https://localhost:7042/api/Budget/balance').then(response => {
        if(response.data) {
          commit('setBudgetBalanceData', response.data);
        }
      })
    }, //TODO DELETE
    fetchTransactions({ commit }) {
      axios.post('https://localhost:7042/api/Transactions').then(response => {
        if(response.data) {
          commit("setTransactions", response.data);
        }
      })
      .catch(error => {
        console.error(error)
      })
    }
  },
});
