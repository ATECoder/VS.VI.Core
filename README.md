# VI Core Project

A library for support and extension of the [National Instruments] (https://www.ni.com) and [IVI] (https://www.ivifoundation.org) VISA.

## Getting Started

Clone the project along with its requisite projects to their respecitve relative path. All projects are installed relative to a common path.

### Prerequisites

The following projects are also required:
* [Core](https://www.bitbucket.org/davidhary/vs.core) - Core Libraries
* [Share](https://www.bitbucket.org/davidhary/vs.share) - Shared snippets
* [Typed Units](https://www.bitbucket.org/davidhary/vs.core) - Typed Units Porject
* [VI](https://www.bitbucket.org/davidhary/vs.vi) - VI Core Project.

```
git clone git@bitbucket.org:davidhary/vs.share.git
git clone git@bitbucket.org:davidhary/vs.core.git
git clone git@bitbucket.org:davidhary/vs.vi.git
git clone git@bitbucket.org:davidhary/vs.typedunits.git
```

### Installing

Install the projects into the following folders:

#### Projects relative path
```
.\Libraries\VS\Share
.\Libraries\VS\Core\Core
.\Libraries\VS\Core\TypedUnits
.\Libraries\VS\VI\Core
```

## Testing

The project includes a few unit test classes. Test applications are under the *Apps* solution folder. 

## Deployment

Deployment projects have not been created for this project.

## Built, Tested and Facilitated By

* [Visual Studio](https://www.visualstudio.com/) - Visual Studio 2015
* [Jarte](https://www.jarte.com/) - RTF Editor
* [Wix Installer](https://www.wixtoolset.org/) - Wix Toolset
* [Atomineer Code Documentation](https://www.atomineerutils.com/) - Code Documentation
* [EW Software](https://github.com/EWSoftware/VSSpellChecker/wiki/) - Spell Checker

## Authors

* **David Hary** - *Initial Workarounds* - [ATE Coder](http://www.isr.cc)

## License

This project is licensed under the MIT License - see the [LICENSE.md] file at (https://www.bitbucket.org/davidhary/vs.vi/src) for details

## Acknowledgments

* Hat tip to anyone who's code was used
* [Its all a remix] (www.everythingisaremix.info) -- we are but a spec on the shoulders of giants
* [Stack overflow] (https://www.stackoveflow.com)
* [National Instruments](https://www.ni.com - NI VISA
* [IVI Foundation](https://www.ivifoundation.org) - VISA

## Revision Changes

* Version 4.0.6123	10/06/16	Throws Format exceptions on failure to parse values with information about the instrument resource and the invalid values. 
