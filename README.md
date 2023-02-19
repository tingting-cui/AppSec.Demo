# Move Share Application

A simple website written to current demonstrate application security mindset and skills.
Suggestions are welcomed.

## Design phase considerations

### Framework choosing

In concern of this app's functionality, I decide using MVC structure. Choosing ASP.NET CORE 7.0(latest version) due to reasons: functionally powerful, well-maintained, security measures well defined and many are directly integrated into functions(e.g. model binding DB interaction, Authorization mechanism, default added Logging function in project initiation and Global Exception Handler sitting at top level pipeline. (More security features check [here](https://learn.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-7.0).

### Access & Privilege design
Application Role 1: Administrator (can perform all actions on all posts Create/Edit/Delete/Order)
Application Role 2: User (default role assigned to any registered accounts, required for Create/Order, can Edit/Delete it's own posts)

Initialized accounts, with same password:A$$leT0ee 
- Alice@moveshare.com (Role Administrator)
- Bob@moveshare.com (Role User)
- Tingting@moveshare.com (Role User)

Database level machine account permissions and view to design [To do].

### Threat modeling & mitigation measures
1 Data corruption risk
- (corrupt at transit/MITM) Enforce HTTPS (set HSTS head effective age = 1 year). [To choose secure cryptographies and avoid fallback]
- (corrupt at storage) Automate MSSQL data corruption check to run regularly. [To do]
- (corrupt from UI/unintentional or malicious) Enforce authN and authZ, validate user input.
- (corrupt from application) Code methods to strictly perform their designed function, well use exception catch and log, Log sensitive actions and all admin actions. [To save log into DB. To add admin action log]
- (application corruption) Sign applicaiton installation package. [To do]

2 Application fall in abnormal status
- Global and local exception handler & log & Ops alert [To design avoiding sensitive info & PII within log]

3 Account take over (brute force)
- Enforce AuthN password complexity policy, least privilege role design.
- Enforce 2FA for all or Administrator account. [To do]
- Detect abnormal login behavior and trigger 2nd channel AuthN or account lockout.
- Upgrade to FIDO2. [To do]

4 CSRF
- Appropriate HTTP method head & validation. 
- CSRF token enforced for sensitive actions and appropriate validation.
- Set AuthN Cookie attribute: same-site=strict
- Server side Access-Control-Allow-Origin, X-Frame-Option:DENY [To do, once register a domain name]
 

5 XSS
- Input validation and sanitization when helpful, at both client side and server side, incl. type and reasonable length, which are designed and enforced when create Model properties.
- Output encoding (Model binding takes care of this aspect).
- Validate data written into and read from database (Model binding takes care of this aspect). 
- Mindfully if have to DOM JS API, don't create opportunity for DOM JS injection.
- CSP, to restrict JS and other resources/images the webpage can load. [To do]

6 Race condition
- Asynchronous methods are used.

## Development phase

### 3rd party library security
Outside the packages and JS libraries ASP.NET Core includes, load two JS from external resources:[Toastr](//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js), and [Bootstrap@5.3.0](//cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.min.js)
CVE info had been checked, and manual review performed before include them.

## Security Test phase - Dynamic testing
- Fuzz input filed, test Reflected XSS, and Stored XSS. 
