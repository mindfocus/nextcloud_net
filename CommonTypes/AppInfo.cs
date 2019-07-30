using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CommonTypes
{
    [XmlRoot(ElementName="types")]
	public class Types {
		[XmlElement(ElementName="filesystem")]
		public string Filesystem { get; set; }
	}

	[XmlRoot(ElementName="nextcloud")]
	public class VersionMinMax {
		[XmlAttribute(AttributeName="min-version")]
		public string Minversion { get; set; }
		[XmlAttribute(AttributeName="max-version")]
		public string Maxversion { get; set; }
	}
	[XmlRoot(ElementName="owncloud")]
	public class Owncloud {
		[XmlAttribute(AttributeName="min-version")]
		public string Minversion { get; set; }
		[XmlAttribute(AttributeName="max-version")]
		public string Maxversion { get; set; }
	}
	[XmlRoot(ElementName="dependencies")]
	public class Dependencies {
		[XmlElement(ElementName="nextcloud")]
		public VersionMinMax Nextcloud { get; set; }
		
		[XmlElement(ElementName="owncloud")]
		public VersionMinMax Owncloud { get; set; }
	}

	[XmlRoot(ElementName="background-jobs")]
	public class Backgroundjobs {
		[XmlElement(ElementName="job")]
		public List<string> Job { get; set; }
	}

	[XmlRoot(ElementName="post-migration")]
	public class Postmigration {
		[XmlElement(ElementName="step")]
		public List<string> Step { get; set; }
	}

	[XmlRoot(ElementName="live-migration")]
	public class Livemigration {
		[XmlElement(ElementName="step")]
		public string Step { get; set; }
	}

	[XmlRoot(ElementName="repair-steps")]
	public class Repairsteps {
		[XmlElement(ElementName="post-migration")]
		public Postmigration Postmigration { get; set; }
		[XmlElement(ElementName="live-migration")]
		public Livemigration Livemigration { get; set; }
	}

	[XmlRoot(ElementName="commands")]
	public class Commands {
		[XmlElement(ElementName="command")]
		public List<string> Command { get; set; }
	}

	[XmlRoot(ElementName="settings")]
	public class Settings {
		[XmlElement(ElementName="admin")]
		public string Admin { get; set; }
		[XmlElement(ElementName="setting")]
		public List<string> Setting { get; set; }
	}

	[XmlRoot(ElementName="filters")]
	public class Filters {
		[XmlElement(ElementName="filter")]
		public List<string> Filter { get; set; }
	}

	[XmlRoot(ElementName="providers")]
	public class Providers {
		[XmlElement(ElementName="provider")]
		public List<string> Provider { get; set; }
	}

	[XmlRoot(ElementName="activity")]
	public class Activity {
		[XmlElement(ElementName="settings")]
		public Settings Settings { get; set; }
		[XmlElement(ElementName="filters")]
		public Filters Filters { get; set; }
		[XmlElement(ElementName="providers")]
		public Providers Providers { get; set; }
	}

	[XmlRoot(ElementName="public")]
	public class Public {
		[XmlElement(ElementName="webdav")]
		public string Webdav { get; set; }
	}

	[XmlRoot(ElementName="info")]
	public class AppInfo {
		[XmlElement(ElementName="id")]
		public string Id { get; set; }
		[XmlElement(ElementName="name")]
		public string Name { get; set; }
		[XmlElement(ElementName="summary")]
		public string Summary { get; set; }
		[XmlElement(ElementName="description")]
		public string Description { get; set; }
		[XmlElement(ElementName="version")]
		public string Version { get; set; }
		[XmlElement(ElementName="licence")]
		public string Licence { get; set; }
		[XmlElement(ElementName="author")]
		public string Author { get; set; }
		[XmlElement(ElementName="namespace")]
		public string Namespace { get; set; }
		[XmlElement(ElementName="default_enable")]
		public string Default_enable { get; set; }
		[XmlElement(ElementName="types")]
		public Types Types { get; set; }
		[XmlElement(ElementName="category")]
		public string Category { get; set; }
		[XmlElement(ElementName="bugs")]
		public string Bugs { get; set; }
		[XmlElement(ElementName="dependencies")]
		public Dependencies Dependencies { get; set; }
		[XmlElement(ElementName="background-jobs")]
		public Backgroundjobs Backgroundjobs { get; set; }
		[XmlElement(ElementName="repair-steps")]
		public Repairsteps Repairsteps { get; set; }
		[XmlElement(ElementName="commands")]
		public Commands Commands { get; set; }
		[XmlElement(ElementName="settings")]
		public Settings Settings { get; set; }
		[XmlElement(ElementName="activity")]
		public Activity Activity { get; set; }
		[XmlElement(ElementName="public")]
		public Public Public { get; set; }
		[XmlAttribute(AttributeName="xsi", Namespace="http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }
		[XmlAttribute(AttributeName="noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string NoNamespaceSchemaLocation { get; set; }
	}
}