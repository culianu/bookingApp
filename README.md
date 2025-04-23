# BookingApp

BookingApp is a program to manage hotel room availability and reservations. The application should read from files containing hotel data and reservation data, then allow a user to check room availability for a specified hotel, date range, and room type.  

## Features

The program implements 2 commands described below and it should exit when a blank line is entered.  
 
 *  **Availability Command**
 
Example console input: 
> Availability(H1, 20240901, SGL) </br>
> Availability(H1, 20240901-20240903, DBL)    
 
Output: the program should give the availability as a number for a room type that date range. Note: hotels sometimes accept overbookings so the value can be negative to indicate this. 

 * **RoomTypes Command**
 
Example console input:  
> RoomTypes(H1, 20240904, 3)  </br>
> RoomTypes(H1, 20240905-20240907, 5)  
 
Output: The program should return a hotel name, and a comma separated list of room type codes needed to allocate number of people specified in the specified time. Minimise the number of rooms to accommodate the required number of people. Avoid partially filling rooms. If a room is partially filled, the room should be marked with a "!”. 


### Prerequisites

- .NET 8 SDK installed on your machine.

### Running the Application

* Navigate to the solution folder
  * #### dotnet run --project bookingApp --hotels <hotels.json> --bookings <bookings.json>
    ### or
  * #### dotnet build and then locate the exe file in bin/debug/net8.0/bookingApp.exe and run bookingApp --hotels <hotels.json> --bookings <bookings.json>
* Instructions on the available commands should appear.



