import { createStore } from 'vuex';

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
    setTransactions(state, data) {
      state.transactions = data;
      state.transactionsLoaded = true;
    },
  },
  actions: {
    fetchBudgetSpentData({ commit }) {
      this.$axios.get('Budget/spent').then(response => {
          if (response.data) {
            commit('setBudgetSpentData', response.data);
          }
        })
        .catch(error => {
          console.error(error);
        });
      },
    fetchBudgetBalanceData({ commit }) {
      this.$axios.get('Budget/balance').then(response => {
        if(response.data) {
          commit('setBudgetBalanceData', response.data);
        }
      })
    },
    addTransaction({ commit }, transaction) {
      commit('setTransactions', [...state.transactions, transaction]);
    },
    calculateBudgetBalance({ commit, state }) {
      let balanceData = {...state.budgetSpentData};
      for (let type in balanceData) {
        if(state.budgetSpentData[type]){
          balanceData[type] = state.budgetSpentData[type] - balanceData[type];
        }
      }
      commit('setBudgetBalanceData', balanceData);
    },
  },
});
