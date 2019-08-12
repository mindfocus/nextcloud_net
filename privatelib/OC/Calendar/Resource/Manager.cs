using OCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OCP.Calendar.Resource;

namespace OC.Calendar.Resource
{
    public class Manager : IManager
    {

    /** @var IServerContainer */
    private IServerContainer server;

    /** @var string[] holds all registered resource backends */
    private IList<string> backends = new List<string>();

    /** @var IBackend[] holds all backends that have been initialized already */
    private IDictionary<string, IBackend> initializedBackends = new Dictionary<string, IBackend>();

    /**
     * Manager constructor.
     *
     * @param IServerContainer $server
     */
    public Manager(IServerContainer server)
    {
        this.server = server;
    }

    /**
     * Registers a resource backend
     *
     * @param string $backendClass
     * @return void
     * @since 14.0.0
     */
    public void registerBackend(string backendClass) {
        //$this.backends[$backendClass] = $backendClass;
    }

    /**
     * Unregisters a resource backend
     *
     * @param string $backendClass
     * @return void
     * @since 14.0.0
     */
    public void unregisterBackend(string backendClass) {
        //unset($this.backends[$backendClass], $this.initializedBackends[$backendClass]);
    }

    /**
     * @return IBackend[]
     * @throws \OCP\AppFramework\QueryException
     * @since 14.0.0
     */
    public IList<IBackend> getBackends(){
        foreach (var backend in backends)
        {
            if (this.initializedBackends.ContainsKey(backend))
            {
                continue;
            }

            this.initializedBackends[backend] = (IBackend) this.server.query(backend);
        }
        return this.initializedBackends.Values.ToList();
    }

    /**
     * @param string $backendId
     * @throws \OCP\AppFramework\QueryException
     * @return IBackend|null
     */
    public IBackend getBackend(string backendId) {
        var backends = this.getBackends();
        foreach (var backend in backends)
        {
            if (backend.getBackendIdentifier() == backendId)
            {
                return backend;
            }
        }
        return null;
    }

    /**
     * removes all registered backend instances
     * @return void
     * @since 14.0.0
     */
    public void clear() {
        this.initializedBackends.Clear();
            this.backends.Clear();
    }
    }

}
