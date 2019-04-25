using OCP;
using System;
using System.Collections.Generic;
using System.Text;
using OCP.Calendar.Resource;

namespace OC.Calendar.Resource
{
    class Manager : OCP.Calendar.Resource.IManager {

    /** @var IServerContainer */
    private IServerContainer server;

    /** @var string[] holds all registered resource backends */
    private IList<string> backends = new List<string>();

    /** @var IBackend[] holds all backends that have been initialized already */
    private IList<IBackend> initializedBackends = new List<IBackend>();

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
        //$this->backends[$backendClass] = $backendClass;
    }

    /**
     * Unregisters a resource backend
     *
     * @param string $backendClass
     * @return void
     * @since 14.0.0
     */
    public void unregisterBackend(string backendClass) {
        //unset($this->backends[$backendClass], $this->initializedBackends[$backendClass]);
    }

    /**
     * @return IBackend[]
     * @throws \OCP\AppFramework\QueryException
     * @since 14.0.0
     */
    public IList<IBackend> getBackends(){
        foreach($this->backends as $backend) {
            if (isset($this->initializedBackends[$backend])) {
                continue;
            }

            $this->initializedBackends[$backend] = $this->server->query($backend);
        }

        return array_values($this->initializedBackends);
    }

    /**
     * @param string $backendId
     * @throws \OCP\AppFramework\QueryException
     * @return IBackend|null
     */
    public function getBackend($backendId) {
        $backends = $this->getBackends();
        foreach($backends as $backend) {
            if ($backend->getBackendIdentifier() === $backendId) {
                return $backend;
            }
        }

        return null;
    }

    /**
     * removes all registered backend instances
     * @return void
     * @since 14.0.0
     */
    public function clear() {
        $this->backends = [];
        $this->initializedBackends = [];
    }
    }

}
