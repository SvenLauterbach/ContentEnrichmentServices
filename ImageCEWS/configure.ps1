#
# Powershell script for configuring the ImageCEWS.
#
Add-PSSnapin "Microsoft.SharePoint.PowerShell" -ErrorAction SilentlyContinue
$ssa = Get-SPEnterpriseSearchServiceApplication

#
# IMPORTANT:
#
# MaxRawDataSize defines the max. file size of  an image passed to the webservice.
# If you modify this value you should also increase the value in the web.config
# <binding maxReceivedMessageSize = "8388608">.
#
# The managed properties listed in "OutputProperties" should already be created 
# before running the CEWS for the first time.
#
# Currently the CEWS only process jpeg files. Modify the Trigger property to
# process other image file types. See http://msdn.microsoft.com/en-us/library/office/jj163983(v=office.15).aspx
# for more information about the trigger expressions. 
#
$config = New-SPEnterpriseSearchContentEnrichmentConfiguration
$config.Endpoint = "http://services.demo.show/ImageCEWS/ImageCEWS.svc"
$config.InputProperties = "FileExtension"
$config.OutputProperties = "comment", "subject"
$config.SendRawData = $True
$config.MaxRawDataSize = 8192 
$config.Trigger = "FileExtension = ""jpeg"" OR FileExtension = ""jpg"""

Set-SPEnterpriseSearchContentEnrichmentConfiguration –SearchApplication $ssa –ContentEnrichmentConfiguration $config