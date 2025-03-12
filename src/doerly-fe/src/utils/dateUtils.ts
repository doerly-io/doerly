export const dateTimeToDateString = (dateTime: string): string => {
  const dateObj = new Date(dateTime);
  const correctedDate = new Date(dateObj.getTime() - dateObj.getTimezoneOffset() * 60000);
  return correctedDate.toISOString().split('T')[0];
}
