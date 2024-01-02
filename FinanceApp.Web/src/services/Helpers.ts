export default class Helpers {
  public static toCamelCase(value: string) {
    if (!value) return "grey";
    return value
      .split("-")
      .map((word: string, index: number) => {
        return index === 0
          ? word
          : word.charAt(0).toUpperCase() + word.slice(1);
      })
      .join("");
  }

  public static formatCurrency(
    amount: number,
    currency = "USD",
    locale = "en-US"
  ) {
    return new Intl.NumberFormat(locale, {
      style: "currency",
      currency: currency,
    }).format(amount);
  }
}
