import {
  Domain, getEnumMeta, solidify, ModelType, ObjectType,
  PrimitiveProperty, ForeignKeyProperty, PrimaryKeyProperty,
  ModelCollectionNavigationProperty, ModelReferenceNavigationProperty,
  HiddenAreas, BehaviorFlags
} from 'coalesce-vue/lib/metadata'


const domain: Domain = { enums: {}, types: {}, services: {} }
export const ApplicationUser = domain.types.ApplicationUser = {
  name: "ApplicationUser",
  displayName: "Application User",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "ApplicationUser",
  get keyProp() { return this.props.applicationUserId }, 
  behaviorFlags: 0 as BehaviorFlags,
  props: {
    applicationUserId: {
      name: "applicationUserId",
      displayName: "Application User Id",
      type: "string",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    azureObjectId: {
      name: "azureObjectId",
      displayName: "Azure Object Id",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Azure Object Id is required.",
      }
    },
    name: {
      name: "name",
      displayName: "Name",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Name is required.",
      }
    },
    email: {
      name: "email",
      displayName: "Email",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Email is required.",
      }
    },
    budgetUsers: {
      name: "budgetUsers",
      displayName: "Budget Users",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.BudgetUser as ModelType) },
      },
      role: "collectionNavigation",
      get foreignKey() { return (domain.types.BudgetUser as ModelType).props.applicationUserId as ForeignKeyProperty },
      get inverseNavigation() { return (domain.types.BudgetUser as ModelType).props.applicationUser as ModelReferenceNavigationProperty },
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const Budget = domain.types.Budget = {
  name: "Budget",
  displayName: "Budget",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "Budget",
  get keyProp() { return this.props.budgetId }, 
  behaviorFlags: 7 as BehaviorFlags,
  props: {
    budgetId: {
      name: "budgetId",
      displayName: "Budget Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    name: {
      name: "name",
      displayName: "Name",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Name is required.",
      }
    },
    description: {
      name: "description",
      displayName: "Description",
      type: "string",
      role: "value",
    },
    budgetUsers: {
      name: "budgetUsers",
      displayName: "Budget Users",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.BudgetUser as ModelType) },
      },
      role: "collectionNavigation",
      get foreignKey() { return (domain.types.BudgetUser as ModelType).props.budgetId as ForeignKeyProperty },
      get inverseNavigation() { return (domain.types.BudgetUser as ModelType).props.budget as ModelReferenceNavigationProperty },
      dontSerialize: true,
    },
    categories: {
      name: "categories",
      displayName: "Categories",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.Category as ModelType) },
      },
      role: "value",
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
    budgetsForUser: {
      type: "dataSource",
      name: "BudgetsForUser",
      displayName: "Budgets For User",
      isDefault: true,
      props: {
      },
    },
  },
}
export const BudgetUser = domain.types.BudgetUser = {
  name: "BudgetUser",
  displayName: "Budget User",
  get displayProp() { return this.props.budgetUserId }, 
  type: "model",
  controllerRoute: "BudgetUser",
  get keyProp() { return this.props.budgetUserId }, 
  behaviorFlags: 0 as BehaviorFlags,
  props: {
    budgetUserId: {
      name: "budgetUserId",
      displayName: "Budget User Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    applicationUserId: {
      name: "applicationUserId",
      displayName: "Application User Id",
      type: "string",
      role: "foreignKey",
      get principalKey() { return (domain.types.ApplicationUser as ModelType).props.applicationUserId as PrimaryKeyProperty },
      get principalType() { return (domain.types.ApplicationUser as ModelType) },
      get navigationProp() { return (domain.types.BudgetUser as ModelType).props.applicationUser as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      rules: {
        required: val => (val != null && val !== '') || "Application User is required.",
      }
    },
    applicationUser: {
      name: "applicationUser",
      displayName: "Application User",
      type: "model",
      get typeDef() { return (domain.types.ApplicationUser as ModelType) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.BudgetUser as ModelType).props.applicationUserId as ForeignKeyProperty },
      get principalKey() { return (domain.types.ApplicationUser as ModelType).props.applicationUserId as PrimaryKeyProperty },
      get inverseNavigation() { return (domain.types.ApplicationUser as ModelType).props.budgetUsers as ModelCollectionNavigationProperty },
      dontSerialize: true,
    },
    budgetId: {
      name: "budgetId",
      displayName: "Budget Id",
      type: "number",
      role: "foreignKey",
      get principalKey() { return (domain.types.Budget as ModelType).props.budgetId as PrimaryKeyProperty },
      get principalType() { return (domain.types.Budget as ModelType) },
      get navigationProp() { return (domain.types.BudgetUser as ModelType).props.budget as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      rules: {
        required: val => val != null || "Budget is required.",
      }
    },
    budget: {
      name: "budget",
      displayName: "Budget",
      type: "model",
      get typeDef() { return (domain.types.Budget as ModelType) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.BudgetUser as ModelType).props.budgetId as ForeignKeyProperty },
      get principalKey() { return (domain.types.Budget as ModelType).props.budgetId as PrimaryKeyProperty },
      get inverseNavigation() { return (domain.types.Budget as ModelType).props.budgetUsers as ModelCollectionNavigationProperty },
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const Category = domain.types.Category = {
  name: "Category",
  displayName: "Category",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "Category",
  get keyProp() { return this.props.categoryId }, 
  behaviorFlags: 7 as BehaviorFlags,
  props: {
    categoryId: {
      name: "categoryId",
      displayName: "Category Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    name: {
      name: "name",
      displayName: "Name",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Name is required.",
      }
    },
    description: {
      name: "description",
      displayName: "Description",
      type: "string",
      role: "value",
    },
    color: {
      name: "color",
      displayName: "Color",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Color is required.",
      }
    },
    icon: {
      name: "icon",
      displayName: "Icon",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Icon is required.",
      }
    },
    subCategories: {
      name: "subCategories",
      displayName: "Sub Categories",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.SubCategory as ModelType) },
      },
      role: "collectionNavigation",
      get foreignKey() { return (domain.types.SubCategory as ModelType).props.categoryId as ForeignKeyProperty },
      get inverseNavigation() { return (domain.types.SubCategory as ModelType).props.category as ModelReferenceNavigationProperty },
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
    categoriesByBudget: {
      type: "dataSource",
      name: "CategoriesByBudget",
      displayName: "Categories By Budget",
      isDefault: true,
      props: {
      },
    },
  },
}
export const CustomCalculation = domain.types.CustomCalculation = {
  name: "CustomCalculation",
  displayName: "Custom Calculation",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "CustomCalculation",
  get keyProp() { return this.props.customCalculationId }, 
  behaviorFlags: 7 as BehaviorFlags,
  props: {
    customCalculationId: {
      name: "customCalculationId",
      displayName: "Custom Calculation Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    name: {
      name: "name",
      displayName: "Name",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Name is required.",
      }
    },
    description: {
      name: "description",
      displayName: "Description",
      type: "string",
      role: "value",
    },
    subCategoryCustomCalculations: {
      name: "subCategoryCustomCalculations",
      displayName: "Sub Category Custom Calculations",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.SubCategoryCustomCalculation as ModelType) },
      },
      role: "collectionNavigation",
      get foreignKey() { return (domain.types.SubCategoryCustomCalculation as ModelType).props.customCalculationId as ForeignKeyProperty },
      get inverseNavigation() { return (domain.types.SubCategoryCustomCalculation as ModelType).props.customCalculation as ModelReferenceNavigationProperty },
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const SubCategory = domain.types.SubCategory = {
  name: "SubCategory",
  displayName: "Sub Category",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "SubCategory",
  get keyProp() { return this.props.subCategoryId }, 
  behaviorFlags: 7 as BehaviorFlags,
  props: {
    subCategoryId: {
      name: "subCategoryId",
      displayName: "Sub Category Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    name: {
      name: "name",
      displayName: "Name",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Name is required.",
      }
    },
    description: {
      name: "description",
      displayName: "Description",
      type: "string",
      role: "value",
    },
    allocation: {
      name: "allocation",
      displayName: "Allocation",
      type: "number",
      role: "value",
      rules: {
        required: val => val != null || "Allocation is required.",
      }
    },
    categoryId: {
      name: "categoryId",
      displayName: "Category Id",
      type: "number",
      role: "foreignKey",
      get principalKey() { return (domain.types.Category as ModelType).props.categoryId as PrimaryKeyProperty },
      get principalType() { return (domain.types.Category as ModelType) },
      get navigationProp() { return (domain.types.SubCategory as ModelType).props.category as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      rules: {
        required: val => val != null || "Category is required.",
      }
    },
    category: {
      name: "category",
      displayName: "Category",
      type: "model",
      get typeDef() { return (domain.types.Category as ModelType) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.SubCategory as ModelType).props.categoryId as ForeignKeyProperty },
      get principalKey() { return (domain.types.Category as ModelType).props.categoryId as PrimaryKeyProperty },
      get inverseNavigation() { return (domain.types.Category as ModelType).props.subCategories as ModelCollectionNavigationProperty },
      dontSerialize: true,
    },
    subCategoryCustomCalculations: {
      name: "subCategoryCustomCalculations",
      displayName: "Sub Category Custom Calculations",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.SubCategoryCustomCalculation as ModelType) },
      },
      role: "collectionNavigation",
      get foreignKey() { return (domain.types.SubCategoryCustomCalculation as ModelType).props.subCategoryId as ForeignKeyProperty },
      get inverseNavigation() { return (domain.types.SubCategoryCustomCalculation as ModelType).props.subCategory as ModelReferenceNavigationProperty },
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const SubCategoryCustomCalculation = domain.types.SubCategoryCustomCalculation = {
  name: "SubCategoryCustomCalculation",
  displayName: "Sub Category Custom Calculation",
  get displayProp() { return this.props.subCategoryCustomCalculationId }, 
  type: "model",
  controllerRoute: "SubCategoryCustomCalculation",
  get keyProp() { return this.props.subCategoryCustomCalculationId }, 
  behaviorFlags: 7 as BehaviorFlags,
  props: {
    subCategoryCustomCalculationId: {
      name: "subCategoryCustomCalculationId",
      displayName: "Sub Category Custom Calculation Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    subCategoryId: {
      name: "subCategoryId",
      displayName: "Sub Category Id",
      type: "number",
      role: "foreignKey",
      get principalKey() { return (domain.types.SubCategory as ModelType).props.subCategoryId as PrimaryKeyProperty },
      get principalType() { return (domain.types.SubCategory as ModelType) },
      get navigationProp() { return (domain.types.SubCategoryCustomCalculation as ModelType).props.subCategory as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      rules: {
        required: val => val != null || "Sub Category is required.",
      }
    },
    subCategory: {
      name: "subCategory",
      displayName: "Sub Category",
      type: "model",
      get typeDef() { return (domain.types.SubCategory as ModelType) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.SubCategoryCustomCalculation as ModelType).props.subCategoryId as ForeignKeyProperty },
      get principalKey() { return (domain.types.SubCategory as ModelType).props.subCategoryId as PrimaryKeyProperty },
      get inverseNavigation() { return (domain.types.SubCategory as ModelType).props.subCategoryCustomCalculations as ModelCollectionNavigationProperty },
      dontSerialize: true,
    },
    customCalculationId: {
      name: "customCalculationId",
      displayName: "Custom Calculation Id",
      type: "number",
      role: "foreignKey",
      get principalKey() { return (domain.types.CustomCalculation as ModelType).props.customCalculationId as PrimaryKeyProperty },
      get principalType() { return (domain.types.CustomCalculation as ModelType) },
      get navigationProp() { return (domain.types.SubCategoryCustomCalculation as ModelType).props.customCalculation as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      rules: {
        required: val => val != null || "Custom Calculation is required.",
      }
    },
    customCalculation: {
      name: "customCalculation",
      displayName: "Custom Calculation",
      type: "model",
      get typeDef() { return (domain.types.CustomCalculation as ModelType) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.SubCategoryCustomCalculation as ModelType).props.customCalculationId as ForeignKeyProperty },
      get principalKey() { return (domain.types.CustomCalculation as ModelType).props.customCalculationId as PrimaryKeyProperty },
      get inverseNavigation() { return (domain.types.CustomCalculation as ModelType).props.subCategoryCustomCalculations as ModelCollectionNavigationProperty },
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const Transaction = domain.types.Transaction = {
  name: "Transaction",
  displayName: "Transaction",
  get displayProp() { return this.props.transactionId }, 
  type: "model",
  controllerRoute: "Transaction",
  get keyProp() { return this.props.transactionId }, 
  behaviorFlags: 7 as BehaviorFlags,
  props: {
    transactionId: {
      name: "transactionId",
      displayName: "Transaction Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    description: {
      name: "description",
      displayName: "Description",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Description is required.",
      }
    },
    amount: {
      name: "amount",
      displayName: "Amount",
      type: "number",
      role: "value",
      rules: {
        required: val => val != null || "Amount is required.",
      }
    },
    subCategoryId: {
      name: "subCategoryId",
      displayName: "Sub Category Id",
      type: "number",
      role: "foreignKey",
      get principalKey() { return (domain.types.SubCategory as ModelType).props.subCategoryId as PrimaryKeyProperty },
      get principalType() { return (domain.types.SubCategory as ModelType) },
      get navigationProp() { return (domain.types.Transaction as ModelType).props.subCategory as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      rules: {
        required: val => val != null || "Sub Category is required.",
      }
    },
    subCategory: {
      name: "subCategory",
      displayName: "Sub Category",
      type: "model",
      get typeDef() { return (domain.types.SubCategory as ModelType) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.Transaction as ModelType).props.subCategoryId as ForeignKeyProperty },
      get principalKey() { return (domain.types.SubCategory as ModelType).props.subCategoryId as PrimaryKeyProperty },
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const UserService = domain.services.UserService = {
  name: "UserService",
  displayName: "User Service",
  type: "service",
  controllerRoute: "UserService",
  methods: {
    getLoggedInUser: {
      name: "getLoggedInUser",
      displayName: "Get Logged In User",
      transportType: "item",
      httpMethod: "POST",
      params: {
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "model",
        get typeDef() { return (domain.types.ApplicationUser as ModelType) },
        role: "value",
      },
    },
  },
}

interface AppDomain extends Domain {
  enums: {
  }
  types: {
    ApplicationUser: typeof ApplicationUser
    Budget: typeof Budget
    BudgetUser: typeof BudgetUser
    Category: typeof Category
    CustomCalculation: typeof CustomCalculation
    SubCategory: typeof SubCategory
    SubCategoryCustomCalculation: typeof SubCategoryCustomCalculation
    Transaction: typeof Transaction
  }
  services: {
    UserService: typeof UserService
  }
}

solidify(domain)

export default domain as unknown as AppDomain
