using System;
using System.Collections;
using System.Collections.Generic;

namespace publicApi.OCP.SystemTag
{
    /**
     * Exception when a tag was not found.
     *
     * @since 9.0.0
     */
    class TagNotFoundException : RuntimeException {

    /** @var string[] */
    protected IList<string> tags;

    /**
     * TagNotFoundException constructor.
     *
     * @param string message
     * @param int code
     * @param \Exception|null previous
     * @param string[] tags
     * @since 9.0.0
     */
    public TagNotFoundException(string message = "", int code = 0, Exception previous = null, IList<string> tags) : base(message, code, previous)
    {
        this.tags = tags;
    }

    /**
     * @return string[]
     * @since 9.0.0
     */
    public IList<string> getMissingTags() {
        return this.tags;
    }
}

}
