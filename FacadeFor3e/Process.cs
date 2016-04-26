using System.Xml;
using JetBrains.Annotations;

// todo: the schema suggests that there are various other properies that could be added, such as:
// Name Description Priority (LOW MEDIUM HIGH) OperatingUnit CheckSum ProxyUser ProxyUserID

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines a process for performing one or more operations on an object
    /// </summary>
    [PublicAPI]
    public class Process : DataObject
        {
        private readonly string _processName;

        /// <summary>
        /// Constructs a new process
        /// </summary>
        /// <param name="processName">Name of the process</param>
        /// <param name="objectName">Name of the object to affect</param>
        public Process(string processName, string objectName)
            : base(objectName)
            {
            this._processName = processName;
            }

        /// <summary>
        /// Returns the process name.
        /// </summary>
        public string ProcessName
            {
            get { return this._processName; }
            }

        /// <summary>
        /// Outputs this process
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        protected internal override void Render(XmlWriter writer)
            {
            string ns0 = string.Format("http://elite.com/schemas/transaction/process/write/{0}", this.ProcessName);
            string ns1 = string.Format("http://elite.com/schemas/transaction/object/write/{0}", this.Name);

            writer.WriteStartDocument();
            writer.WriteStartElement(this._processName, ns0);
            writer.WriteStartElement("Initialize", ns1);
            foreach (OperationBase o in this.Operations)
                {
                o.Render(writer, this.Name);
                }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            }

        /// <summary>
        /// Generate the XML instructions to pass to 3e
        /// </summary>
        /// <returns>The transaction to be carried out</returns>
        public XmlDocument GenerateCommand()
            {
            var xmlDoc = new XmlDocument();
            using (XmlWriter w = xmlDoc.CreateNavigator().AppendChild())
                {
                Render(w);
                }
            return xmlDoc;
            }

        // ----- static constructors - add your own as required

        /// <summary>
        /// Constructs a new Matter process
        /// </summary>
        /// <returns>A process for updating matters</returns>
        public static Process NewMatterProcess()
            {
            var result = new Process("N_WsMatter", "Matter");
            return result;
            }

        /// <summary>
        /// Constructs a new Entity process
        /// </summary>
        /// <returns>A process for updating entities</returns>
        public static Process NewEntityProcess()
            {
            var result = new Process("N_WsEntity", "Entity");
            return result;
            }

        /// <summary>
        /// Constructs a new Entity Organisation process
        /// </summary>
        /// <returns>A process for updating organisation entities</returns>
        public static Process NewEntityProcessOrganisation()
            {
            var result = new Process("N_WsEntityOrganisation", "EntityOrg");
            return result;
            }

        /// <summary>
        /// Constructs a new Entity Person process
        /// </summary>
        /// <returns>A process for updating person entities</returns>
        public static Process NewEntityProcessPerson()
            {
            var result = new Process("N_WsEntityPerson", "EntityPerson");
            return result;
            }

        /// <summary>
        /// Constructs a new Timekeeper process
        /// </summary>
        /// <returns>A process for updating timekeepers</returns>
        public static Process NewTimekeeperProcess()
            {
            var result = new Process("N_WsTimekeeper", "Timekeeper");
            return result;
            }

        /// <summary>
        /// Constructs a new Client process
        /// </summary>
        /// <returns>A process for updating Clients</returns>
        public static Process NewClientProcess()
            {
            var result = new Process("N_WsClient", "Client");
            return result;
            }

        /// <summary>
        /// Constructs a new ESBTimeCardLoad process
        /// </summary>
        /// <returns>A process for updating matters</returns>
        public static Process NewEsbTimeCardLoadProcess()
            {
            var result = new Process("ESBTimeCardLoad", "TimeCardPending");
            return result;
            }

        /// <summary>
        /// Constructs a new ESBCostCardLoad process
        /// </summary>
        /// <returns>A process for updating matters</returns>
        public static Process NewEsbCostCardLoadProcess()
            {
            var result = new Process("ESBCostCardLoad", "CostCardPending");
            return result;
            }
        }
    }
